﻿using System;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace ReactiveCore
{
    [DataContract]
    public class ReactiveObject : IReactiveObject, IReactiveNotifyPropertyChanged<IReactiveObject>
    {
        #region events
        public event PropertyChangingEventHandler PropertyChanging;
        public event PropertyChangedEventHandler PropertyChanged;

        void IReactiveObject.RaisePropertyChanging(PropertyChangingEventArgs args) => this.PropertyChanging?.Invoke(this, args);

        void IReactiveObject.RaisePropertyChanged(PropertyChangedEventArgs args) => this.PropertyChanged?.Invoke(this, args);

        #endregion

        /// <summary>
        /// Represents an Observable that fires *before* a property is about to
        /// be changed.
        /// </summary>
        [IgnoreDataMember]
        public IObservable<IReactivePropertyChangedEventArgs<IReactiveObject>> Changing
        {
            get { return ((IReactiveObject)this).getChangingObservable(); }
        }

        /// <summary>
        /// Represents an Observable that fires *after* a property has changed.
        /// </summary>
        [IgnoreDataMember]
        public IObservable<IReactivePropertyChangedEventArgs<IReactiveObject>> Changed
        {
            get { return ((IReactiveObject)this).getChangedObservable(); }
        }

        /// <summary>
        ///
        /// </summary>
        [IgnoreDataMember]
        public IObservable<Exception> ThrownExceptions { get { return this.getThrownExceptionsObservable(); } }

        public ReactiveObject()
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IDisposable SuppressChangeNotifications()
        {
            return this.suppressChangeNotifications();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public bool AreChangeNotificationsEnabled()
        {
            return this.areChangeNotificationsEnabled();
        }

        public IDisposable DelayChangeNotifications()
        {
            return this.delayChangeNotifications();
        }
    }

}
