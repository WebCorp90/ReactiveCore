using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace ReactiveDbCore
{
    public class ValidationEntityError
    {
       
       

        public ValidationEntityError(IReactiveDbObject entity, ValidationException ex)
        {
            Contract.Requires(entity != null);
            Contract.Requires(ex != null);
            this.Entity = entity;
            this.Exception = ex;
        }

        public IReactiveDbObject Entity { get; private set; }
        public ValidationException Exception { get; private set; }

    }
}