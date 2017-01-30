using Microsoft.EntityFrameworkCore;
using PropertyChangedCore.Helpers;
using ReactiveCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
namespace ReactiveDbCore
{
    /// <summary>
	/// A <see cref="DbContext"/>-derived class with trigger functionality called automatically
    /// <see cref="https://github.com/NickStrupat/EntityFramework.Triggers"/>
    /// Add some functionnality for triggering events on saving processus
	/// </summary>
    public class ReactiveDbContext : DbContext, IReactiveObject
    {
        #region fields
        ISubject<IReactiveDbContextEventArgs> errorSubject;
        IObservable<IReactiveDbContextEventArgs> errorObservable;
        private Countable errorCountable;
        
        
        #endregion

        #region Constructors
        public ReactiveDbContext() : base() { Initialize(); }
        public ReactiveDbContext(DbContextOptions options) : base(options) { Initialize(); }
        private void Initialize()
        {
            this.errorSubject = new Subject<IReactiveDbContextEventArgs>();
            this.errorObservable = errorSubject.Publish().RefCount();
            this.errorCountable = new Countable();
            this.errorObservable = this.errorCountable.GetCountable(this.errorObservable);
        }


        #endregion

        #region Properties
        [Reactive]
        public bool TriggersEnabled { get; set; } = true;
        #endregion

        #region SaveChanges
        public override int SaveChanges() => this.SaveChanges(true);

        public override int SaveChanges(bool acceptAllChangesOnSuccess)=> TriggersEnabled ? this.SaveChangesWithTriggers(base.SaveChanges, acceptAllChangesOnSuccess) : base.SaveChanges(acceptAllChangesOnSuccess);
        

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken)) => TriggersEnabled ? this.SaveChangesWithTriggersAsync(base.SaveChangesAsync, cancellationToken) : base.SaveChangesAsync(cancellationToken);

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken)) => TriggersEnabled ? this.SaveChangesWithTriggersAsync(base.SaveChangesAsync, cancellationToken) : base.SaveChangesAsync(cancellationToken);

        #endregion

        #region IReactiveObject
        public event PropertyChangingEventHandler PropertyChanging;
        public event PropertyChangedEventHandler PropertyChanged;

        void IReactiveObject.RaisePropertyChanging(PropertyChangingEventArgs args) => this.PropertyChanging?.Invoke(this, args);

        void IReactiveObject.RaisePropertyChanged(PropertyChangedEventArgs args) => this.PropertyChanged?.Invoke(this, args);

        public IObservable<IReactivePropertyChangedEventArgs<IReactiveObject>> Changing => ((IReactiveObject)this).getChangingObservable();

        public IObservable<IReactivePropertyChangedEventArgs<IReactiveObject>> Changed => ((IReactiveObject)this).getChangedObservable();


        #endregion

        #region ERROR
        public IObservable<IReactiveDbContextEventArgs> Error => this.errorObservable;
        public int ErrorCountSubscriber => errorCountable.Count;
        public void RaiseError(Exception ex)
        {
            errorSubject.OnNext(new ReactiveDbContextEventArgs(this, ex));
        }
        #endregion

        #region Validation
        public bool EnableValidation { get; set; } = true;
        public void Validate()
        {
            if (!EnableValidation) return;
            var entities = from e in this.ChangeTracker.Entries()
                           where e.State == EntityState.Added
                               || e.State == EntityState.Modified
                           select e.Entity;
            List<ValidationEntityError> errors = new List<ValidationEntityError>();
            foreach (var entity in entities)
            {
                try
                {
                    var validationContext = new ValidationContext(entity);
                    Validator.ValidateObject(entity, validationContext);
                }catch(ValidationException ex)
                {
                    errors.Add(new ValidationEntityError(entity, ex));
                    //throw new ValidationEntityException(new ValidationEntityEventArg(this, entity, ex));
                }
            }
            if(errors.Count>0) throw new ValidationEntityException(new ValidationEntityEventArg(this,errors));
        }
        #endregion

    }
}
