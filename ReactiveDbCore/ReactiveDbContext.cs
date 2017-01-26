using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PropertyChangedCore.Helpers;
using ReactiveCore;

namespace ReactiveDbCore
{
    /// <summary>
	/// A <see cref="DbContext"/>-derived class with trigger functionality called automatically
    /// <see cref="https://github.com/NickStrupat/EntityFramework.Triggers"/>
    /// Add some functionnality for triggering events on saving processus
	/// </summary>
    public class ReactiveDbContext : DbContext, IReactiveObject
    {

        #region Constructors
        public ReactiveDbContext() : base() { }
        public ReactiveDbContext(DbContextOptions options) : base(options) { }
        #endregion

        #region Properties
        [Reactive]
        public bool TriggersEnabled { get; set; } = true;
        #endregion

        #region SaveChanges
        public override int SaveChanges()=> TriggersEnabled ? this.SaveChangesWithTriggers(base.SaveChanges) : base.SaveChanges();
        
        public override int SaveChanges(bool acceptAllChangesOnSuccess)=> TriggersEnabled ? this.SaveChangesWithTriggers(base.SaveChanges, acceptAllChangesOnSuccess) : base.SaveChanges(acceptAllChangesOnSuccess);

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))=>TriggersEnabled ? this.SaveChangesWithTriggersAsync(base.SaveChangesAsync, cancellationToken) : base.SaveChangesAsync(cancellationToken);

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))=>TriggersEnabled ? this.SaveChangesWithTriggersAsync(base.SaveChangesAsync, cancellationToken) : base.SaveChangesAsync(cancellationToken);
        
        #endregion

        #region IReactiveObject
        public event PropertyChangingEventHandler PropertyChanging;
        public event PropertyChangedEventHandler PropertyChanged;

        void IReactiveObject.RaisePropertyChanging(PropertyChangingEventArgs args) => this.PropertyChanging?.Invoke(this, args);

        void IReactiveObject.RaisePropertyChanged(PropertyChangedEventArgs args) => this.PropertyChanged?.Invoke(this, args);

        public IObservable<IReactivePropertyChangedEventArgs<IReactiveObject>> Changing => ((IReactiveObject)this).getChangingObservable();

        public IObservable<IReactivePropertyChangedEventArgs<IReactiveObject>> Changed => ((IReactiveObject)this).getChangedObservable();


        #endregion

    }
}
