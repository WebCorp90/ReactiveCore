using ReactiveCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReactiveDbCore
{
    public interface IReactiveDbObject : IReactiveObject, INotifyEntityAdded, INotifyEntityAdding
    {
        void RaiseEntityAdded(ReactiveDbEventArgs args);
        void RaiseEntityAdding(ReactiveDbEventArgs args);

    }

    public static class IReactiveDbObjectExtensions
    {
        static ConditionalWeakTable<IReactiveDbObject, IExtensionState> state = new ConditionalWeakTable<IReactiveDbObject, IExtensionState>();

        public static IObservable<IReactiveDbEventArgs> getAddedObservable(this IReactiveDbObject This) 
        {
            var val = state.GetValue(This, key => (IExtensionState)new ExtensionState(This));
            return val.Added.Cast<IReactiveDbEventArgs>();
        }

        public static IObservable<IReactiveDbEventArgs> getAddingObservable(this IReactiveDbObject This) 
        {
            var val = state.GetValue(This, key => (IExtensionState)new ExtensionState(This));
            return val.Adding.Cast<IReactiveDbEventArgs>();
        }

        static IEnumerable<IReactiveDbEventArgs> dedup(IList<IReactiveDbEventArgs> batch)
        {
            if (batch.Count <= 1)
            {
                return batch;
            }

            var seen = new HashSet<string>();
            var unique = new LinkedList<IReactiveDbEventArgs>();

            for (int i = batch.Count - 1; i >= 0; i--)
            {
                if (seen.Add(batch[i].PropertyName))
                {
                    unique.AddFirst(batch[i]);
                }
            }

            return unique;
        }

        class ExtensionState : IExtensionState
        {
            long changeNotificationsSuppressed;
            long changeNotificationsDelayed;
            ISubject<IReactiveDbEventArgs> addingSubject;
            IObservable<IReactiveDbEventArgs> addingObservable;
            ISubject<IReactiveDbEventArgs> addedSubject;
            IObservable<IReactiveDbEventArgs> addedObservable;
            ISubject<IReactiveDbEventArgs> fireChangedBatchSubject;
            ISubject<Exception> thrownExceptions;
            ISubject<Unit> startDelayNotifications;

            IReactiveDbObject sender;

            /// <summary>
            /// Initializes a new instance of the <see cref="ExtensionState{TSender}"/> class.
            /// </summary>
            public ExtensionState(IReactiveDbObject sender)
            {
                this.sender = sender;
                this.addingSubject = new Subject<IReactiveDbEventArgs>();
                this.addedSubject = new Subject<IReactiveDbEventArgs>();
                this.startDelayNotifications = new Subject<Unit>();
                this.thrownExceptions = new ScheduledSubject<Exception>(Scheduler.Immediate, ReactiveCoreApp.DefaultExceptionHandler);

                this.addedObservable = addedSubject
                    .Buffer(
                        Observable.Merge(
                            addedSubject.Where(_ => !areChangeNotificationsDelayed()).Select(_ => Unit.Default),
                            startDelayNotifications)
                    )
                    .SelectMany(batch => dedup(batch))
                    .Publish()
                    .RefCount();

                this.addingObservable = addingSubject
                    .Buffer(
                        Observable.Merge(
                            addingSubject.Where(_ => !areChangeNotificationsDelayed()).Select(_ => Unit.Default),
                            startDelayNotifications)
                    )
                    .SelectMany(batch => dedup(batch))
                    .Publish()
                    .RefCount();
            }

            public IObservable<IReactiveDbEventArgs> Adding
            {
                get { return this.addingObservable; }
            }

            public IObservable<IReactiveDbEventArgs> Added
            {
                get { return this.addedObservable; }
            }

            public IObservable<Exception> ThrownExceptions
            {
                get { return thrownExceptions; }
            }

            public bool areChangeNotificationsEnabled()
            {
                return (Interlocked.Read(ref changeNotificationsSuppressed) == 0);
            }

            public bool areChangeNotificationsDelayed()
            {
                return (Interlocked.Read(ref changeNotificationsDelayed) > 0);
            }

            /// <summary>
            /// When this method is called, an object will not fire change
            /// notifications (neither traditional nor Observable notifications)
            /// until the return value is disposed.
            /// </summary>
            /// <returns>An object that, when disposed, reenables change
            /// notifications.</returns>
            public IDisposable suppressChangeNotifications()
            {
                Interlocked.Increment(ref changeNotificationsSuppressed);
                return Disposable.Create(() => Interlocked.Decrement(ref changeNotificationsSuppressed));
            }

            public IDisposable delayChangeNotifications()
            {
                if (Interlocked.Increment(ref changeNotificationsDelayed) == 1)
                {
                    startDelayNotifications.OnNext(Unit.Default);
                }

                return Disposable.Create(() => {
                    if (Interlocked.Decrement(ref changeNotificationsDelayed) == 0)
                    {
                        startDelayNotifications.OnNext(Unit.Default);
                    };
                });
            }

            public void raiseEntityAdding(string propertyName)
            {
                if (!this.areChangeNotificationsEnabled())
                    return;

                var adding = new ReactiveDbEventArgs(sender, propertyName);
                sender.RaiseEntityAdding(adding);

                this.notifyObservable(sender, adding, this.addingSubject);
            }

            public void raiseEntityAdded(string propertyName)
            {
                if (!this.areChangeNotificationsEnabled())
                    return;

                var added = new ReactiveDbEventArgs(sender, propertyName);
                sender.RaiseEntityAdding(added);

                this.notifyObservable(sender, added, this.addedSubject);
            }

            internal void notifyObservable<T>(IReactiveDbObject rxObj, T item, ISubject<T> subject)
            {
                try
                {
                    subject.OnNext(item);
                }
                catch (Exception ex)
                {
                    // rxObj.Log().ErrorException("ReactiveObject Subscriber threw exception", ex);
                    thrownExceptions.OnNext(ex);
                }
            }
        }

        interface IExtensionState
        {
            IObservable<IReactiveDbEventArgs> Adding { get; }

            IObservable<IReactiveDbEventArgs> Added { get; }

            void raiseEntityAdding(string propertyName);

            void raiseEntityAdded(string propertyName);

            IObservable<Exception> ThrownExceptions { get; }

            bool areChangeNotificationsEnabled();

            IDisposable suppressChangeNotifications();

            bool areChangeNotificationsDelayed();

            IDisposable delayChangeNotifications();
        }

    }


}
