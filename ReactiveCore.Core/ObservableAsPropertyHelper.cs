﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Disposables;

namespace ReactiveCore
{
    /// <summary>
    /// ObservableAsPropertyHelper is a class to help ViewModels implement
    /// "output properties", that is, a property that is backed by an
    /// Observable. The property will be read-only, but will still fire change
    /// notifications. This class can be created directly, but is more often created via the
    /// ToProperty and ObservableToProperty extension methods.
    /// </summary>
    public sealed class ObservableAsPropertyHelper<T> :  IDisposable//, IEnableLogger
    {
        T _lastValue;
        readonly IConnectableObservable<T> _source;
        IDisposable _inner;

        /// <summary>
        /// Constructs an ObservableAsPropertyHelper object.
        /// </summary>
        /// <param name="observable">The Observable to base the property on.</param>
        /// <param name="onChanged">The action to take when the property
        /// changes, typically this will call the ViewModel's
        /// RaisePropertyChanged method.</param>
        /// <param name="initialValue">The initial value of the property.</param>
        /// <param name="scheduler">The scheduler that the notifications will be
        /// provided on - this should normally be a Dispatcher-based scheduler
        /// (and is by default)</param>
        public ObservableAsPropertyHelper(
            IObservable<T> observable,
            Action<T> onChanged,
            T initialValue = default(T),
            IScheduler scheduler = null) : this(observable, onChanged, null, initialValue, scheduler) { }

        /// <summary>
        /// Constructs an ObservableAsPropertyHelper object.
        /// </summary>
        /// <param name="observable">The Observable to base the property on.</param>
        /// <param name="onChanged">The action to take when the property
        /// changes, typically this will call the ViewModel's
        /// RaisePropertyChanged method.</param>
        /// <param name="onChanging">The action to take when the property
        /// changes, typically this will call the ViewModel's
        /// RaisePropertyChanging method.</param>
        /// <param name="initialValue">The initial value of the property.</param>
        /// <param name="scheduler">The scheduler that the notifications will be
        /// provided on - this should normally be a Dispatcher-based scheduler
        /// (and is by default)</param>
        public ObservableAsPropertyHelper(
            IObservable<T> observable,
            Action<T> onChanged,
            Action<T> onChanging = null,
            T initialValue = default(T),
            IScheduler scheduler = null)
        {
            Contract.Requires(observable != null);
            Contract.Requires(onChanged != null);

            scheduler = scheduler ?? CurrentThreadScheduler.Instance;
            onChanging = onChanging ?? (_ => { });
            _lastValue = initialValue;

            var subj = new ScheduledSubject<T>(scheduler);
            var exSubject = new ScheduledSubject<Exception>(CurrentThreadScheduler.Instance, ReactiveCoreApp.DefaultExceptionHandler);

            bool firedInitial = false;
            subj.Subscribe(x => {
                // Suppress a non-change between initialValue and the first value
                // from a Subscribe
                if (firedInitial && EqualityComparer<T>.Default.Equals(x, _lastValue)) return;

                onChanging(x);
                _lastValue = x;
                onChanged(x);
                firedInitial = true;
            }, exSubject.OnNext);

            ThrownExceptions = exSubject;

            // Fire off an initial RaisePropertyChanged to make sure bindings
            // have a value
            subj.OnNext(initialValue);
            _source = observable.DistinctUntilChanged().Multicast(subj);

           /* if (ModeDetector.InUnitTestRunner())
            {
                _inner = _source.Connect();
            }*/
        }

        /// <summary>
        /// The last provided value from the Observable. 
        /// </summary>
        public T Value
        {
            get
            {
                _inner = _inner ?? _source.Connect();
                return _lastValue;
            }
        }

        /// <summary>
        /// Fires whenever an exception would normally terminate ReactiveUI 
        /// internal state.
        /// </summary>
        public IObservable<Exception> ThrownExceptions { get; private set; }

        public void Dispose()
        {
            (_inner ?? Disposable.Empty).Dispose();
            _inner = null;
        }

        /// <summary>
        /// Constructs a "default" ObservableAsPropertyHelper object. This is
        /// useful for when you will initialize the OAPH later, but don't want
        /// bindings to access a null OAPH at startup.
        /// </summary>
        /// <param name="initialValue">The initial (and only) value of the property.</param>
        /// <param name="scheduler">The scheduler that the notifications will be
        /// provided on - this should normally be a Dispatcher-based scheduler
        /// (and is by default)</param>
        public static ObservableAsPropertyHelper<T> Default(T initialValue = default(T), IScheduler scheduler = null)
        {
            return new ObservableAsPropertyHelper<T>(Observable.Never<T>(), _ => { }, initialValue, scheduler);
        }
    }

    public static class OAPHCreationHelperMixin
    {
        static ObservableAsPropertyHelper<TRet> observableToProperty<TObj, TRet>(
                this TObj This,
                IObservable<TRet> observable,
                Expression<Func<TObj, TRet>> property,
                TRet initialValue = default(TRet),
                IScheduler scheduler = null)
            where TObj : ReactiveObject
        {
            Contract.Requires(This != null);
            Contract.Requires(observable != null);
            Contract.Requires(property != null);

            Expression expression = Reflection.Rewrite(property.Body);

            if (expression.GetParent().NodeType != ExpressionType.Parameter)
            {
                throw new ArgumentException("Property expression must be of the form 'x => x.SomeProperty'");
            }

            var name = expression.GetMemberInfo().Name;
            var ret = new ObservableAsPropertyHelper<TRet>(observable,
                _ => This.raisePropertyChanged(name),
                _ => This.raisePropertyChanging(name),
                initialValue, scheduler);

            return ret;
        }

        /// <summary>
        /// Converts an Observable to an ObservableAsPropertyHelper and
        /// automatically provides the onChanged method to raise the property
        /// changed notification.         
        /// </summary>
        /// <param name="source">The ReactiveObject that has the property</param>
        /// <param name="property">An Expression representing the property (i.e.
        /// 'x => x.SomeProperty'</param>
        /// <param name="initialValue">The initial value of the property.</param>
        /// <param name="scheduler">The scheduler that the notifications will be
        /// provided on - this should normally be a Dispatcher-based scheduler
        /// (and is by default)</param>
        /// <returns>An initialized ObservableAsPropertyHelper; use this as the
        /// backing field for your property.</returns>
        public static ObservableAsPropertyHelper<TRet> ToProperty<TObj, TRet>(
            this IObservable<TRet> This,
            TObj source,
            Expression<Func<TObj, TRet>> property,
            TRet initialValue = default(TRet),
            IScheduler scheduler = null)
            where TObj : ReactiveObject
        {
            return source.observableToProperty(This, property, initialValue, scheduler);
        }

        /// <summary>
        /// Converts an Observable to an ObservableAsPropertyHelper and
        /// automatically provides the onChanged method to raise the property
        /// changed notification.         
        /// </summary>
        /// <param name="source">The ReactiveObject that has the property</param>
        /// <param name="property">An Expression representing the property (i.e.
        /// 'x => x.SomeProperty'</param>
        /// <param name="initialValue">The initial value of the property.</param>
        /// <param name="scheduler">The scheduler that the notifications will be
        /// provided on - this should normally be a Dispatcher-based scheduler
        /// (and is by default)</param>
        /// <returns>An initialized ObservableAsPropertyHelper; use this as the
        /// backing field for your property.</returns>
        public static ObservableAsPropertyHelper<TRet> ToProperty<TObj, TRet>(
            this IObservable<TRet> This,
            TObj source,
            Expression<Func<TObj, TRet>> property,
            out ObservableAsPropertyHelper<TRet> result,
            TRet initialValue = default(TRet),
            IScheduler scheduler = null)
            where TObj : ReactiveObject
        {
            var ret = source.observableToProperty(This, property, initialValue, scheduler);

            result = ret;
            return ret;
        }
    }
}
