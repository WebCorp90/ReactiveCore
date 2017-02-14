using ReactiveCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ReactiveDbCore
{
    public interface IReactiveDbContext : IReactiveObject
    {
        bool TriggersEnabled { get; set; }

#if CORE
        int SaveChanges() ;

       int SaveChanges(bool acceptAllChangesOnSuccess) ;

        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken ) ;

        Task<int> SaveChangesAsync(CancellationToken cancellationToken ) ;
#else
        int SaveChanges(bool acceptAllChangesOnSuccess);

        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken);

        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
#endif
        #region ERRORS
        IObservable<IReactiveDbContextEventArgs> Error { get; }

        int ErrorCountSubscriber { get; }

        void RaiseError(Exception ex);
        void DetachEntity<TEntity>(TEntity x) where TEntity : ReactiveDbObject;

        #endregion

        #region Validation
        IObservable<ValidationEntitiesException> ValidationError { get; }
        int ValidationErrorCountSubscriber { get; }


        bool EnableValidation { get; set; }
        bool ForceNoTracking { get; set; }

        void RaiseValidationError(ValidationEntitiesException ex);
        void Validate();

        void Attach<TEntity>(TEntity x) where TEntity : ReactiveDbObject;
        void ChangeState<TEntity>(TEntity entity, EntityState unchanged) where TEntity : ReactiveDbObject;
        #endregion

    }
}
