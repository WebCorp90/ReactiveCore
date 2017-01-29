using ReactiveCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveDbCore
{

    public interface IReactiveDbObjectEventArgs
    {


        /// <summary>
        /// The object that has raised the change.
        /// </summary>
        IReactiveDbObject Sender { get; }

        Exception Exception { get; }
    }
    public interface IReactiveDbContextEventArgs
    {


        /// <summary>
        /// The object that has raised the change.
        /// </summary>
        ReactiveDbContext Sender { get; }

        Exception Exception { get; }
    }
    public class ReactiveDbObjectEventArgs : EventArgs, IReactiveDbObjectEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReactivePropertyChangedEventArgs{TSender}"/> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        public ReactiveDbObjectEventArgs(IReactiveDbObject sender):this(sender,null)
        {
            
        }

        public ReactiveDbObjectEventArgs(IReactiveDbObject sender,Exception ex)
        {
            this.Sender = sender;
            this.Exception = ex;
        }

        public Exception Exception { get; private set; }

        /// <summary>
        ///
        /// </summary>
        public IReactiveDbObject Sender { get; private set; }
    }

    public class ReactiveDbContextEventArgs : EventArgs, IReactiveDbContextEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReactivePropertyChangedEventArgs{TSender}"/> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        public ReactiveDbContextEventArgs(ReactiveDbContext sender) : this(sender, null)
        {

        }

        public ReactiveDbContextEventArgs(ReactiveDbContext sender, Exception ex)
        {
            this.Sender = sender;
            this.Exception = ex;
        }

        public Exception Exception { get; private set; }

        /// <summary>
        ///
        /// </summary>
        public ReactiveDbContext Sender { get; private set; }
    }

    public delegate void ReactiveDbEventHandler(IReactiveDbObject sender, ReactiveDbObjectEventArgs e);
    public interface INotifyEntityAdded
    {
        event ReactiveDbEventHandler EntityAdded;
    }
    public interface INotifyEntityAdding
    {
        event ReactiveDbEventHandler EntityAdding;
    }
}
