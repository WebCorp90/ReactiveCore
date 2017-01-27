using ReactiveCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveDbCore
{

    public interface IReactiveDbEventArgs
    {


        /// <summary>
        /// The object that has raised the change.
        /// </summary>
        IReactiveDbObject Sender { get; }
    }
    public class ReactiveDbEventArgs : EventArgs, IReactiveDbEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReactivePropertyChangedEventArgs{TSender}"/> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="propertyName">Name of the property.</param>
        public ReactiveDbEventArgs(IReactiveDbObject sender)
        {
            this.Sender = sender;
        }

       
        /// <summary>
        ///
        /// </summary>
        public IReactiveDbObject Sender { get; private set; }
    }
    

    public delegate void ReactiveDbEventHandler(IReactiveDbObject sender, ReactiveDbEventArgs e);
    public interface INotifyEntityAdded
    {
        event ReactiveDbEventHandler EntityAdded;
    }
    public interface INotifyEntityAdding
    {
        event ReactiveDbEventHandler EntityAdding;
    }
}
