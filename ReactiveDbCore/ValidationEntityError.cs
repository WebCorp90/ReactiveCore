using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace ReactiveDbCore
{
    public class ValidationEntityError
    {
       
       

        public ValidationEntityError(object entity, ValidationException ex)
        {
            Contract.Requires(entity != null);
            Contract.Requires(ex != null);
            this.Entity = entity;
            this.Exception = ex;
        }

        public object Entity { get; private set; }
        public ValidationException Exception { get; private set; }

    }
}