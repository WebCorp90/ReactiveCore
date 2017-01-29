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
        void RaiseEntityAdded(ReactiveDbObjectEventArgs args);
        void RaiseEntityAdding(ReactiveDbObjectEventArgs args);
        void RaiseEntityUpdating(ReactiveDbObjectEventArgs args);
        void RaiseEntityUpdated(ReactiveDbObjectEventArgs args);
        void RaiseEntityDeleting(ReactiveDbObjectEventArgs args);
        void RaiseEntityDeleted(ReactiveDbObjectEventArgs args);
        void RaiseEntityError(ReactiveDbObjectEventArgs args);
    }

    public static class IReactiveDbObjectExtensions
    {
        static ConditionalWeakTable<IReactiveDbObject, IExtensionState> state = new ConditionalWeakTable<IReactiveDbObject, IExtensionState>();

        #region ADD
        public static IObservable<IReactiveDbObjectEventArgs> getAddedObservable(this IReactiveDbObject This) 
        {
            var val = state.GetValue(This, key => (IExtensionState)new ExtensionState(This));
            return val.Added.Cast<IReactiveDbObjectEventArgs>();
        }

        public static IObservable<IReactiveDbObjectEventArgs> getAddingObservable(this IReactiveDbObject This) 
        {
            var val = state.GetValue(This, key => (IExtensionState)new ExtensionState(This));
            return val.Adding.Cast<IReactiveDbObjectEventArgs>();
        }

        internal static void RaiseDbEntityAdding(this IReactiveDbObject entity)
        {
            var s = state.GetValue(entity, key => (IExtensionState)new ExtensionState(entity));

            s.raiseEntityAdding();
        }

        internal static void RaiseDbEntityAdded(this IReactiveDbObject entity)
        {
            var s = state.GetValue(entity, key => (IExtensionState)new ExtensionState(entity));

            s.raiseEntityAdded();
        }
        #endregion

        #region UPDATE
        public static IObservable<IReactiveDbObjectEventArgs> getUpdatedObservable(this IReactiveDbObject This)
        {
            var val = state.GetValue(This, key => (IExtensionState)new ExtensionState(This));
            return val.Updated.Cast<IReactiveDbObjectEventArgs>();
        }

        public static IObservable<IReactiveDbObjectEventArgs> getUpdatingObservable(this IReactiveDbObject This)
        {
            var val = state.GetValue(This, key => (IExtensionState)new ExtensionState(This));
            return val.Updating.Cast<IReactiveDbObjectEventArgs>();
        }

        internal static void RaiseDbEntityUpdating(this IReactiveDbObject entity)
        {
            var s = state.GetValue(entity, key => (IExtensionState)new ExtensionState(entity));

            s.raiseEntityUpdating();
        }

        internal static void RaiseDbEntityUpdated(this IReactiveDbObject entity)
        {
            var s = state.GetValue(entity, key => (IExtensionState)new ExtensionState(entity));

            s.raiseEntityUpdated();
        }
        #endregion

        #region DELETE
        public static IObservable<IReactiveDbObjectEventArgs> getDeletedObservable(this IReactiveDbObject This)
        {
            var val = state.GetValue(This, key => (IExtensionState)new ExtensionState(This));
            return val.Deleted.Cast<IReactiveDbObjectEventArgs>();
        }

        public static IObservable<IReactiveDbObjectEventArgs> getDeletingObservable(this IReactiveDbObject This)
        {
            var val = state.GetValue(This, key => (IExtensionState)new ExtensionState(This));
            return val.Deleting.Cast<IReactiveDbObjectEventArgs>();
        }

        internal static void RaiseDbEntityDeleting(this IReactiveDbObject entity)
        {
            var s = state.GetValue(entity, key => (IExtensionState)new ExtensionState(entity));

            s.raiseEntityDeleting();
        }

        internal static void RaiseDbEntityDeleted(this IReactiveDbObject entity)
        {
            var s = state.GetValue(entity, key => (IExtensionState)new ExtensionState(entity));

            s.raiseEntityDeleted();
        }
        #endregion

        #region ERROR
        public static IObservable<IReactiveDbObjectEventArgs> getErrorObservable(this IReactiveDbObject This)
        {
            var val = state.GetValue(This, key => (IExtensionState)new ExtensionState(This));
            return val.Error.Cast<IReactiveDbObjectEventArgs>();
        }
        internal static bool RaiseDbEntityError(this IReactiveDbObject entity,Exception ex)
        {
            var s = state.GetValue(entity, key => (IExtensionState)new ExtensionState(entity));

            return s.raiseEntityError(ex);
        }

        internal static bool RaiseDbContextError(this ReactiveDbContext context,Exception ex)
        {
            if (context.ErrorCountSubscriber == 0) return false;
            context.RaiseError(ex);
            return true;
        }
        #endregion
        #region internal helpers methods
        static IEnumerable<IReactiveDbObjectEventArgs> dedup(IList<IReactiveDbObjectEventArgs> batch)
        {
            if (batch.Count <= 1)
            {
                return batch;
            }

            var seen = new HashSet<IReactiveDbObject>();
            var unique = new LinkedList<IReactiveDbObjectEventArgs>();

            for (int i = batch.Count - 1; i >= 0; i--)
            {
                if (seen.Add(batch[i].Sender))
                {
                    unique.AddFirst(batch[i]);
                }
            }

            return unique;
        }
        #endregion

        #region internal Helpers Classes
        class ExtensionState : IExtensionState
        {
            long changeNotificationsSuppressed;
            long changeNotificationsDelayed;
            ISubject<IReactiveDbObjectEventArgs> addingSubject;
            IObservable<IReactiveDbObjectEventArgs> addingObservable;
            ISubject<IReactiveDbObjectEventArgs> addedSubject;
            IObservable<IReactiveDbObjectEventArgs> addedObservable;
            ISubject<IReactiveDbObjectEventArgs> updatingSubject;
            IObservable<IReactiveDbObjectEventArgs> updatingObservable;
            ISubject<IReactiveDbObjectEventArgs> updatedSubject;
            IObservable<IReactiveDbObjectEventArgs> updatedObservable;
            ISubject<IReactiveDbObjectEventArgs> deletingSubject;
            IObservable<IReactiveDbObjectEventArgs> deletingObservable;
            ISubject<IReactiveDbObjectEventArgs> deletedSubject;
            IObservable<IReactiveDbObjectEventArgs> deletedObservable;
            ISubject<IReactiveDbObjectEventArgs> errorSubject;
            IObservable<IReactiveDbObjectEventArgs> errorObservable;

            ISubject<IReactiveDbObjectEventArgs> fireChangedBatchSubject;
            ISubject<Exception> thrownExceptions;
            ISubject<Unit> startDelayNotifications;

            IReactiveDbObject sender;

            Countable errorCountable;
            IObservable<IReactiveDbObjectEventArgs> errorCount;
            /// <summary>
            /// Initializes a new instance of the <see cref="ExtensionState{TSender}"/> class.
            /// </summary>
            public ExtensionState(IReactiveDbObject sender)
            {
                this.sender = sender;
                this.addingSubject = new Subject<IReactiveDbObjectEventArgs>();
                this.addedSubject = new Subject<IReactiveDbObjectEventArgs>();
                this.updatingSubject = new Subject<IReactiveDbObjectEventArgs>();
                this.updatedSubject = new Subject<IReactiveDbObjectEventArgs>();
                this.deletingSubject = new Subject<IReactiveDbObjectEventArgs>();
                this.deletedSubject = new Subject<IReactiveDbObjectEventArgs>();
                this.errorSubject = new Subject<IReactiveDbObjectEventArgs>();
                this.startDelayNotifications = new Subject<Unit>();
                this.thrownExceptions = new ScheduledSubject<Exception>(Scheduler.Immediate, ReactiveCoreApp.DefaultExceptionHandler);

               /* this.addedObservable = createObservable(ref addedSubject);
                this.addingObservable = createObservable(ref addingSubject);
                this.updatedObservable = createObservable(ref updatedSubject);
                this.updatingObservable = createObservable(ref updatingSubject);
                this.deletedObservable = createObservable(ref deletedSubject);
                this.deletingObservable = createObservable(ref deletingSubject);*/
                
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

                this.updatedObservable = updatedSubject
                    .Buffer(
                        Observable.Merge(
                            updatedSubject.Where(_ => !areChangeNotificationsDelayed()).Select(_ => Unit.Default),
                            startDelayNotifications)
                    )
                    .SelectMany(batch => dedup(batch))
                    .Publish()
                    .RefCount();

                this.updatingObservable = updatingSubject
                    .Buffer(
                        Observable.Merge(
                            updatingSubject.Where(_ => !areChangeNotificationsDelayed()).Select(_ => Unit.Default),
                            startDelayNotifications)
                    )
                    .SelectMany(batch => dedup(batch))
                    .Publish()
                    .RefCount();

                this.deletedObservable = deletedSubject
                    .Buffer(
                        Observable.Merge(
                            deletedSubject.Where(_ => !areChangeNotificationsDelayed()).Select(_ => Unit.Default),
                            startDelayNotifications)
                    )
                    .SelectMany(batch => dedup(batch))
                    .Publish()
                    .RefCount();

                this.deletingObservable = deletingSubject
                    .Buffer(
                        Observable.Merge(
                            deletingSubject.Where(_ => !areChangeNotificationsDelayed()).Select(_ => Unit.Default),
                            startDelayNotifications)
                    )
                    .SelectMany(batch => dedup(batch))
                    .Publish()
                    .RefCount();
                
                this.errorObservable = errorSubject
                    .Buffer(
                        Observable.Merge(
                            errorSubject.Where(_ => !areChangeNotificationsDelayed()).Select(_ => Unit.Default),
                            startDelayNotifications)
                    )
                    .SelectMany(batch => dedup(batch))
                    .Publish()
                    .RefCount();
                this.errorCountable = new Countable();
                this.errorObservable = this.errorCountable.GetCountable(this.errorObservable);

            }

          /*  private IObservable<IReactiveDbEventArgs> createObservable(ref ISubject<IReactiveDbEventArgs> subject)
            {
                return subject.Buffer(
                        Observable.Merge(
                            addingSubject.Where(_ => !areChangeNotificationsDelayed()).Select(_ => Unit.Default),
                            startDelayNotifications)
                    )
                    .SelectMany(batch => dedup(batch))
                    .Publish()
                    .RefCount();
            }*/

            public IObservable<IReactiveDbObjectEventArgs> Adding => this.addingObservable;
            
            public IObservable<IReactiveDbObjectEventArgs> Added=> this.addedObservable;

            public IObservable<IReactiveDbObjectEventArgs> Updating => this.updatingObservable;

            public IObservable<IReactiveDbObjectEventArgs> Updated => this.updatedObservable;

            public IObservable<IReactiveDbObjectEventArgs> Deleting => this.deletingObservable;

            public IObservable<IReactiveDbObjectEventArgs> Deleted => this.deletedObservable;

            public IObservable<IReactiveDbObjectEventArgs> Error => this.errorObservable;

            public IObservable<Exception> ThrownExceptions=> thrownExceptions;

            //public int errorCountSubscriber { get; private set; }

            public bool areChangeNotificationsEnabled()=> (Interlocked.Read(ref changeNotificationsSuppressed) == 0);

            public bool areChangeNotificationsDelayed()=> (Interlocked.Read(ref changeNotificationsDelayed) > 0);

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

            public void raiseEntityAdding()=> raiseEntityEvent(sender.RaiseEntityAdding, this.addingSubject);

            public void raiseEntityAdded() => raiseEntityEvent(sender.RaiseEntityAdded, this.addedSubject);

            public void raiseEntityUpdating()=> raiseEntityEvent(sender.RaiseEntityUpdating, this.updatingSubject);
            
            public void raiseEntityUpdated()=> raiseEntityEvent(sender.RaiseEntityUpdated, this.updatedSubject);

            public void raiseEntityDeleting() => raiseEntityEvent(sender.RaiseEntityDeleting, this.deletingSubject);

            public void raiseEntityDeleted() => raiseEntityEvent(sender.RaiseEntityDeleted, this.deletedSubject);

            public bool raiseEntityError(Exception ex)
            {
                if (errorCountable.Count == 0) return false;
                raiseEntityEvent(sender.RaiseEntityError, this.errorSubject, ex);
                return true;
            }

            private void raiseEntityEvent(Action<ReactiveDbObjectEventArgs> @event,ISubject<IReactiveDbObjectEventArgs> @subject,Exception ex=null)
            {
                if (!this.areChangeNotificationsEnabled())
                    return;
                
                var args = new ReactiveDbObjectEventArgs(sender,ex);
                @event(args);
                this.notifyObservable(sender, args, subject);
            }
            private void notifyObservable<T>(IReactiveDbObject rxObj, T item, ISubject<T> subject)
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
            #region ADD
            IObservable<IReactiveDbObjectEventArgs> Adding { get; }

            IObservable<IReactiveDbObjectEventArgs> Added { get; }

            void raiseEntityAdding();

            void raiseEntityAdded();
            #endregion

            #region UPDATE
            IObservable<IReactiveDbObjectEventArgs> Updating { get; }

            IObservable<IReactiveDbObjectEventArgs> Updated { get; }

            void raiseEntityUpdating();

            void raiseEntityUpdated();
            #endregion

            #region DELETE
            IObservable<IReactiveDbObjectEventArgs> Deleting { get; }

            IObservable<IReactiveDbObjectEventArgs> Deleted { get; }

            void raiseEntityDeleting();

            void raiseEntityDeleted();
            #endregion

            #region ERROR
            IObservable<IReactiveDbObjectEventArgs> Error { get; }

            bool raiseEntityError(Exception ex);
            
            #endregion

            IObservable<Exception> ThrownExceptions { get; }

            bool areChangeNotificationsEnabled();

            IDisposable suppressChangeNotifications();

            bool areChangeNotificationsDelayed();

            IDisposable delayChangeNotifications();
        }


        #endregion
    }


}
