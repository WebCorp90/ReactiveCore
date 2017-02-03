using ReactiveCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveDbCore
{
    public interface IValidationEntityEventArg
    {
        ValidationEntityError Error { get; }
    }
    public interface IValidationEntitiesEventArg
    {

        ReactiveDbContext Context { get; }
        List<ValidationEntityError> Errors { get; }
    }
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

    public class ValidationEntitiesEventArg:EventArgs,IValidationEntitiesEventArg
    {
   

        public ValidationEntitiesEventArg(ReactiveDbContext context, List<ValidationEntityError> errors)
        {
            this.Context = context;
            this.Errors = errors;
        }

        public ReactiveDbContext Context { get; private set; }

        public List<ValidationEntityError> Errors { get; private set; }
    }

    public class ValidationEntitiesException : Exception
    {
        public ValidationEntitiesException(ValidationEntitiesEventArg args)
        {
            this.Errors = args;
        }

        public ValidationEntitiesEventArg Errors { get; private set; }
    }


    public class ValidationEntityEventArg : EventArgs, IValidationEntityEventArg
    {


        public ValidationEntityEventArg( ValidationEntityError error)
        {
            this.Error = error;
        }
        

        public ValidationEntityError Error { get; private set; }
    }

    public class ValidationEntityException : Exception
    {
        public ValidationEntityException(IReactiveDbObject sender, ValidationEntityEventArg args)
        {
            this.Errors = args;
            this.Sender = sender;
        }

        public ValidationEntityEventArg Errors { get; private set; }
        public IReactiveDbObject Sender { get; private set; }
    }

    public delegate void ReactiveDbEventHandler(IReactiveDbObject sender, ReactiveDbObjectEventArgs e);

    public delegate void ValidationEntityEventHandler(ValidationEntityEventArg e);

    public interface INotifyEntityAdded
    {
        event ReactiveDbEventHandler onAdded;
    }
    public interface INotifyEntityAdding
    {
        event ReactiveDbEventHandler onAdding;
    }
}
