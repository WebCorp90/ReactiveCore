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
        void IReactiveDbObject.RaiseEntityAdded(ReactiveDbEventArgs args) => this.EntityAdded?.Invoke(this, args);

        public event ReactiveDbEventHandler EntityAdding;
        void IReactiveDbObject.RaiseEntityAdding(ReactiveDbEventArgs args) => this.EntityAdding?.Invoke(this, args);
        

        #endregion

        /// <summary>
        /// Represents an Observable that fires *before* a property is about to
        /// be adding.
        /// </summary>
        [IgnoreDataMember]
        public IObservable<IReactiveDbEventArgs> Adding => ((IReactiveDbObject)this).getAddingObservable();


        /// <summary>
        /// Represents an Observable that fires *after* a property is added
        /// </summary>
        [IgnoreDataMember]
        public IObservable<IReactiveDbEventArgs> Added => ((IReactiveDbObject)this).getAddingObservable();



      
        
    }
}
