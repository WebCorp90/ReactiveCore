

using System;
using System.Collections.Generic;

using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#if CORE
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection;
#else
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
#endif

#if DEBUG
using System.Diagnostics;
#endif
namespace ReactiveDbCore
{
    internal class EntityEntryComparer : IEqualityComparer<DbEntityEntry>
    {
        private EntityEntryComparer() { }
#if CORE
        public bool Equals(EntityEntry x, EntityEntry y) => ReferenceEquals(x.Entity, y.Entity);
        public int GetHashCode(EntityEntry obj) => obj.Entity.GetHashCode();
#else
        public bool Equals(DbEntityEntry x, DbEntityEntry y) => ReferenceEquals(x.Entity, y.Entity);

        public int GetHashCode(DbEntityEntry obj) => obj.Entity.GetHashCode();
#endif
        public static readonly EntityEntryComparer Default = new EntityEntryComparer();
    }

    internal static class DbContextExtensions
    {


        public static int SaveChangesWithTriggers<TReactiveDbContext>(this TReactiveDbContext dbContext, Func<bool, int> baseSaveChanges, bool acceptAllChangesOnSuccess = true) where TReactiveDbContext : ReactiveDbContext
        {
            Contract.Requires(dbContext != null);
            Contract.Requires(baseSaveChanges != null);


            try
            {
                dbContext.Validate();
                var afterEvents = dbContext.RaiseBeforeEvents();
                var result = baseSaveChanges(acceptAllChangesOnSuccess);
                afterEvents.RaiseAfterEvents();
                return result;
            }
            catch (ValidationEntitiesException ex) when (dbContext.RaiseValidationFailedEvents(ex)) { }
            catch (Exception ex) when (dbContext.RaiseFailedEvents(ex)) { }
            return 0;


        }

        private static List<Action> RaiseBeforeEvents<TReactiveDbContext>(this TReactiveDbContext dbContext) where TReactiveDbContext : ReactiveDbContext
        {
            
            Contract.Requires(dbContext != null);
            Contract.Ensures(Contract.Result<List<Action>>() != null);
            var entries = dbContext.GetReactiveDbObjectEntries();
#if CORE
            var triggeredEntries = new List<EntityEntry>(entries.Count);
#else
            var triggeredEntries = new List<DbEntityEntry>(entries.Count);
#endif
            List<Action> afterEvents = new List<Action>(entries.Count);
            while (entries.Any())
            {
                foreach (var entry in entries)
                {
                    var events = entry.GetPairEvent();
                    events.Before();
                    afterEvents.Add(events.After);

                    triggeredEntries.Add(entry);
                }
                var newEntries = dbContext.GetReactiveDbObjectEntries().Except(triggeredEntries, EntityEntryComparer.Default);
                entries.Clear();
                entries.AddRange(newEntries);
            }
            return afterEvents;
        }

        private static void RaiseAfterEvents(this List<Action> events)
        {
            events.ForEach(
                @event =>
                {
                    @event();

                });
        }

        private static bool RaiseFailedEvents<TReactiveDbContext>(this TReactiveDbContext context, Exception ex) where TReactiveDbContext : ReactiveDbContext
        {
            Contract.Requires(context != null);
            Contract.Requires(ex != null);
            var entityResult = false;
            var contextResult = false;
            contextResult = context.RaiseDbContextError(ex);
            context.GetReactiveDbObjectEntries().ForEach(entry =>
            {
                entityResult = ((IReactiveDbObject)entry.Entity).RaiseDbEntityError(ex) && true;
            });
            return contextResult || entityResult;
        }


        private static bool RaiseValidationFailedEvents<TReactiveDbcontext>(this TReactiveDbcontext context, ValidationEntitiesException ex) where TReactiveDbcontext : ReactiveDbContext
        {
            Contract.Requires(context != null);
            Contract.Requires(ex != null);
            var entityResult = false;
            var contextResult = false;
#if DEBUG
            if (Debugger.IsAttached) Debugger.Break();
#endif
            contextResult = context.RaiseDbValidationContextError(ex);
            ex.Errors.Errors.ForEach(entry =>
            {

                entityResult = ((IReactiveDbObject)entry.Entity).RaiseDbValidationEntityError(entry.Exception) && true;

            });
            return contextResult || entityResult;
        }
#if CORE
        public static List<EntityEntry> GetReactiveDbObjectEntries<TReactiveDbContext>(this TReactiveDbContext dbContext) where TReactiveDbContext : ReactiveDbContext
            => dbContext.ChangeTracker.Entries().Where(e => { return typeof(IReactiveDbObject).GetTypeInfo().IsAssignableFrom(e.Entity.GetType()); }).ToList();
        
        public static List<EntityEntry> GetValidatableReactiveDbObjectEntries<TReactiveDbContext>(this TReactiveDbContext dbContext) where TReactiveDbContext : ReactiveDbContext
            => dbContext.GetReactiveDbObjectEntries().Where(e => { return e.State == EntityState.Added || e.State == EntityState.Modified; }).ToList();

        public static BeforeAfterDbEvent<TEntry> GetPairEvent<TEntry>(this TEntry entry) where TEntry : EntityEntry
            => new BeforeAfterDbEvent<TEntry>(entry);
#else
        public static List<DbEntityEntry> GetReactiveDbObjectEntries<TReactiveDbContext>(this TReactiveDbContext dbContext) where TReactiveDbContext : ReactiveDbContext
        => dbContext.ChangeTracker.Entries().Where(e => { return typeof(IReactiveDbObject).IsAssignableFrom(e.Entity.GetType()); }).ToList();

        public static List<DbEntityEntry> GetValidatableReactiveDbObjectEntries<TReactiveDbContext>(this TReactiveDbContext dbContext) where TReactiveDbContext : ReactiveDbContext
            => dbContext.GetReactiveDbObjectEntries().Where(e => { return e.State == EntityState.Added || e.State == EntityState.Modified; }).ToList();

        public static BeforeAfterDbEvent<TEntry> GetPairEvent<TEntry>(this TEntry entry) where TEntry : DbEntityEntry
           => new BeforeAfterDbEvent<TEntry>(entry);
#endif






        public static async Task<int> SaveChangesWithTriggersAsync<TReactiveDbContext>(this TReactiveDbContext dbContext, Func<Boolean, CancellationToken, Task<Int32>> baseSaveChangesAsync, Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
            where TReactiveDbContext : ReactiveDbContext
        {
            return await Task.Run(() =>
            {
                return 1;
            });
        }
        public static Task<Int32> SaveChangesWithTriggersAsync<TReactiveDbContext>(this TReactiveDbContext dbContext, Func<Boolean, CancellationToken, Task<Int32>> baseSaveChangesAsync, CancellationToken cancellationToken = default(CancellationToken))
            where TReactiveDbContext : ReactiveDbContext
        {
            return dbContext.SaveChangesWithTriggersAsync(baseSaveChangesAsync, true, cancellationToken);
        }

       
    }
    public class BeforeAfterDbEvent<TEntry> where TEntry :
 #if CORE
        EntityEntry
#else
        DbEntityEntry
#endif
    {
        public BeforeAfterDbEvent(TEntry entry)
        {
            this.Entry = entry;
            switch (entry.State)
            {
                case EntityState.Detached:
                    break;
                case EntityState.Unchanged:
                    break;
                case EntityState.Deleted:
                    Before = ((IReactiveDbObject)entry.Entity).RaiseDbEntityDeleting;
                    After = ((IReactiveDbObject)entry.Entity).RaiseDbEntityDeleted;
                    break;
                case EntityState.Modified:
                    Before = ((IReactiveDbObject)entry.Entity).RaiseDbEntityUpdating;
                    After = ((IReactiveDbObject)entry.Entity).RaiseDbEntityUpdated;
                    break;
                case EntityState.Added:
                    Before = ((IReactiveDbObject)entry.Entity).RaiseDbEntityAdding;
                    After = ((IReactiveDbObject)entry.Entity).RaiseDbEntityAdded;
                    break;
                default:
                    break;
            }
        }

        public Action Before { get; private set; } = null;
        public Action After { get; private set; } = null;
        public TEntry Entry { get; private set; }

    }
}