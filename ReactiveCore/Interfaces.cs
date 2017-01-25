using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace ReactiveCore
{
    /// <summary>
    /// IObservedChange is a generic interface that is returned from WhenAny()
    /// Note that it is used for both Changing (i.e.'before change')
    /// and Changed Observables.
    /// </summary>
    public interface IObservedChange<out TSender, out TValue>
    {
        /// <summary>
        /// The object that has raised the change.
        /// </summary>
        TSender Sender { get; }

        /// <summary>
        /// The expression of the member that has changed on Sender.
        /// </summary>
        Expression Expression { get; }

        /// <summary>
        /// The value of the property that has changed. IMPORTANT NOTE: This
        /// property is often not set for performance reasons, unless you have
        /// explicitly requested an Observable for a property via a method such
        /// as ObservableForProperty. To retrieve the value for the property,
        /// use the GetValue() extension method.
        /// </summary>
        TValue Value { get; }
    }

    /// <summary>
    /// A data-only version of IObservedChange
    /// </summary>
    public class ObservedChange<TSender, TValue> : IObservedChange<TSender, TValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObservedChange{TSender, TValue}"/> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="expression">Expression describing the member.</param>
        /// <param name="value">The value.</param>
        public ObservedChange(TSender sender, Expression expression, TValue value = default(TValue))
        {
            this.Sender = sender;
            this.Expression = expression;
            this.Value = value;
        }

        /// <summary>
        ///
        /// </summary>
        public TSender Sender { get; private set; }

        /// <summary>
        ///
        /// </summary>
        public Expression Expression { get; private set; }

        /// <summary>
        ///
        /// </summary>
        public TValue Value { get; private set; }
    }

    /// <summary>
    /// IReactivePropertyChangedEventArgs is a generic interface that
    /// is used to wrap the NotifyPropertyChangedEventArgs and gives
    /// information about changed properties. It includes also
    /// the sender of the notification.
    /// Note that it is used for both Changing (i.e.'before change')
    /// and Changed Observables.
    /// </summary>
    public interface IReactivePropertyChangedEventArgs<out TSender>
    {
        /// <summary>
        /// The name of the property that has changed on Sender.
        /// </summary>
        string PropertyName { get; }

        /// <summary>
        /// The object that has raised the change.
        /// </summary>
        TSender Sender { get; }
    }

    /// <summary>
    /// IReactiveNotifyPropertyChanged represents an extended version of
    /// INotifyPropertyChanged that also exposes typed Observables.
    /// </summary>
    public interface IReactiveNotifyPropertyChanged<out TSender>
    {
        /// <summary>
        /// Represents an Observable that fires *before* a property is about to
        /// be changed. Note that this should not fire duplicate change notifications if a
        /// property is set to the same value multiple times.
        /// </summary>
        IObservable<IReactivePropertyChangedEventArgs<TSender>> Changing { get; }

        /// <summary>
        /// Represents an Observable that fires *after* a property has changed.
        /// Note that this should not fire duplicate change notifications if a
        /// property is set to the same value multiple times.
        /// </summary>
        IObservable<IReactivePropertyChangedEventArgs<TSender>> Changed { get; }

        /// <summary>
        /// When this method is called, an object will not fire change
        /// notifications (neither traditional nor Observable notifications)
        /// until the return value is disposed.
        /// </summary>
        /// <returns>An object that, when disposed, reenables change
        /// notifications.</returns>
        IDisposable SuppressChangeNotifications();
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TSender"></typeparam>
    public class ReactivePropertyChangingEventArgs<TSender> : PropertyChangingEventArgs, IReactivePropertyChangedEventArgs<TSender>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReactivePropertyChangingEventArgs{TSender}"/> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="propertyName">Name of the property.</param>
        public ReactivePropertyChangingEventArgs(TSender sender, string propertyName)
            : base(propertyName)
        {
            this.Sender = sender;
        }

        /// <summary>
        ///
        /// </summary>
        public TSender Sender { get; private set; }
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TSender"></typeparam>
    public class ReactivePropertyChangedEventArgs<TSender> : PropertyChangedEventArgs, IReactivePropertyChangedEventArgs<TSender>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReactivePropertyChangedEventArgs{TSender}"/> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="propertyName">Name of the property.</param>
        public ReactivePropertyChangedEventArgs(TSender sender, string propertyName)
            : base(propertyName)
        {
            this.Sender = sender;
        }

        /// <summary>
        ///
        /// </summary>
        public TSender Sender { get; private set; }
    }
}
