using ReactiveCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveDbCore
{
    public class ReactiveDbObject : ReactiveObject,IReactiveDbObject
    {
        #region events
        public event ReactiveDbEventHandler onAdded;
        void IReactiveDbObject.RaiseEntityAdded(ReactiveDbObjectEventArgs args) => this.onAdded?.Invoke(this, args);

        public event ReactiveDbEventHandler onAdding;
        void IReactiveDbObject.RaiseEntityAdding(ReactiveDbObjectEventArgs args) => this.onAdding?.Invoke(this, args);

        public event ReactiveDbEventHandler onUpdated;
        void IReactiveDbObject.RaiseEntityUpdated(ReactiveDbObjectEventArgs args) => this.onUpdated?.Invoke(this, args);

        public event ReactiveDbEventHandler onUpdating;
        void IReactiveDbObject.RaiseEntityUpdating(ReactiveDbObjectEventArgs args) => this.onUpdating?.Invoke(this, args);

        public event ReactiveDbEventHandler onDeleted;
        void IReactiveDbObject.RaiseEntityDeleted(ReactiveDbObjectEventArgs args) => this.onDeleted?.Invoke(this, args);

        public event ReactiveDbEventHandler onDeleting;
        void IReactiveDbObject.RaiseEntityDeleting(ReactiveDbObjectEventArgs args) => this.onDeleting?.Invoke(this, args);

        public event ReactiveDbEventHandler onError;
        void IReactiveDbObject.RaiseEntityError(ReactiveDbObjectEventArgs args) => this.onError?.Invoke(this, args);


        public event ValidationEntityEventHandler onValidationError;
        void IReactiveDbObject.RaiseEntityValidationError(ValidationEntityEventArg args) => this.onValidationError?.Invoke( args);

        #endregion

        /// <summary>
        /// Represents an Observable that fires *before* a property is about to
        /// be adding.
        /// </summary>
        [IgnoreDataMember]
        public IObservable<IReactiveDbObjectEventArgs> Adding => ((IReactiveDbObject)this).getAddingObservable();


        /// <summary>
        /// Represents an Observable that fires *after* a property is added
        /// </summary>
        [IgnoreDataMember]
        public IObservable<IReactiveDbObjectEventArgs> Added => ((IReactiveDbObject)this).getAddedObservable();


        /// <summary>
        /// Represents an Observable that fires *before* a property is about to
        /// be updating.
        /// </summary>
        [IgnoreDataMember]
        public IObservable<IReactiveDbObjectEventArgs> Updating => ((IReactiveDbObject)this).getUpdatingObservable();


        /// <summary>
        /// Represents an Observable that fires *after* a property is updated
        /// </summary>
        [IgnoreDataMember]
        public IObservable<IReactiveDbObjectEventArgs> Updated => ((IReactiveDbObject)this).getUpdatedObservable();

        /// <summary>
        /// Represents an Observable that fires *before* a property is about to
        /// be deleting.
        /// </summary>
        [IgnoreDataMember]
        public IObservable<IReactiveDbObjectEventArgs> Deleting => ((IReactiveDbObject)this).getDeletingObservable();


        /// <summary>
        /// Represents an Observable that fires *after* a property is deleted
        /// </summary>
        [IgnoreDataMember]
        public IObservable<IReactiveDbObjectEventArgs> Deleted => ((IReactiveDbObject)this).getDeletedObservable();

        /// <summary>
        /// Represents an Observable that fires *after* a property is on error
        /// </summary>
        [IgnoreDataMember]
        public IObservable<IReactiveDbObjectEventArgs> Error => ((IReactiveDbObject)this).getErrorObservable();

        /// <summary>
        /// Represents an Observable that fires *after* a property is on error
        /// </summary>
        [IgnoreDataMember]
        public IObservable<ValidationEntityEventArg> ValidationError => ((IReactiveDbObject)this).getValidationErrorObservable();

    }
}
