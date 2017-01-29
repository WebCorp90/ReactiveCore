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
        public event ReactiveDbEventHandler EntityAdded;
        void IReactiveDbObject.RaiseEntityAdded(ReactiveDbObjectEventArgs args) => this.EntityAdded?.Invoke(this, args);

        public event ReactiveDbEventHandler EntityAdding;
        void IReactiveDbObject.RaiseEntityAdding(ReactiveDbObjectEventArgs args) => this.EntityAdding?.Invoke(this, args);

        public event ReactiveDbEventHandler EntityUpdated;
        void IReactiveDbObject.RaiseEntityUpdated(ReactiveDbObjectEventArgs args) => this.EntityUpdated?.Invoke(this, args);

        public event ReactiveDbEventHandler EntityUpdating;
        void IReactiveDbObject.RaiseEntityUpdating(ReactiveDbObjectEventArgs args) => this.EntityUpdating?.Invoke(this, args);

        public event ReactiveDbEventHandler EntityDeleted;
        void IReactiveDbObject.RaiseEntityDeleted(ReactiveDbObjectEventArgs args) => this.EntityDeleted?.Invoke(this, args);

        public event ReactiveDbEventHandler EntityDeleting;
        void IReactiveDbObject.RaiseEntityDeleting(ReactiveDbObjectEventArgs args) => this.EntityDeleting?.Invoke(this, args);

        public event ReactiveDbEventHandler EntityError;
        void IReactiveDbObject.RaiseEntityError(ReactiveDbObjectEventArgs args) => this.EntityError?.Invoke(this, args);



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

    }
}
