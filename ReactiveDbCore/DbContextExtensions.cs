using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
namespace ReactiveDbCore
{
    internal class EntityEntryComparer : IEqualityComparer<EntityEntry>
    {
        private EntityEntryComparer() { }
        public bool Equals(EntityEntry x, EntityEntry y) => ReferenceEquals(x.Entity, y.Entity);
        public int GetHashCode(EntityEntry obj) => obj.Entity.GetHashCode();
        public static readonly EntityEntryComparer Default = new EntityEntryComparer();
    }

    internal static class DbContextExtensions
    {


        public static int SaveChangesWithTriggers<TReactiveDbContext>(this TReactiveDbContext dbContext, Func<bool, int> baseSaveChanges, bool acceptAllChangesOnSuccess = true) where TReactiveDbContext : ReactiveDbContext
        {
            Contract.Requires(dbContext != null);
            Contract.Requires(baseSaveChanges != null);

            var afterEvents = dbContext.RaiseBeforeEvents();
            try
            {
                var result = baseSaveChanges(acceptAllChangesOnSuccess);
                afterEvents.RaiseAfterEvents();
                return result;
            }
            catch (Exception ex) when (dbContext.RaiseFailedEvents(ex))
            {

            }
            return 0;


        }

        private static List<Action> RaiseBeforeEvents<TReactiveDbContext>(this TReactiveDbContext dbContext) where TReactiveDbContext : ReactiveDbContext
        {
            Contract.Requires(dbContext != null);
            Contract.Ensures(Contract.Result<List<Action>>() != null);
            var entries = dbContext.GetReactiveDbObjectEntries();
            var triggeredEntries = new List<EntityEntry>(entries.Count);
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
            context.RaiseDbContextError(ex);
            context.GetReactiveDbObjectEntries().ForEach(entry =>
            {
                entityResult = ((IReactiveDbObject)entry.Entity).RaiseDbEntityError(ex) && true;
            });
            return contextResult || entityResult;
        }

        public static List<EntityEntry> GetReactiveDbObjectEntries<TReactiveDbContext>(this TReactiveDbContext dbContext) where TReactiveDbContext : ReactiveDbContext => dbContext.ChangeTracker.Entries().Where(e => { return typeof(IReactiveDbObject).IsAssignableFrom(e.Entity.GetType()); }).ToList();


        public static async Task<int> SaveChangesWithTriggersAsync<TReactiveDbContext>(this TReactiveDbContext dbContext, Func<Boolean, CancellationToken, Task<Int32>> baseSaveChangesAsync, Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Task.Run(() =>
            {
                return 1;
            });
        }
        public static Task<Int32> SaveChangesWithTriggersAsync<TReactiveDbContext>(this TReactiveDbContext dbContext, Func<Boolean, CancellationToken, Task<Int32>> baseSaveChangesAsync, CancellationToken cancellationToken = default(CancellationToken))
        {
            return dbContext.SaveChangesWithTriggersAsync(baseSaveChangesAsync, true, cancellationToken);
        }

        public static BeforeAfterDbEvent<TEntry> GetPairEvent<TEntry>(this TEntry entry) where TEntry : EntityEntry
        => new BeforeAfterDbEvent<TEntry>(entry);
    }
    public class BeforeAfterDbEvent<TEntry> where TEntry : EntityEntry
    {
        public BeforeAfterDbEvent(TEntry entry)
        {
            this.Entry = entry;
            switch (entry.State)
            {
                case Microsoft.EntityFrameworkCore.EntityState.Detached:
                    break;
                case Microsoft.EntityFrameworkCore.EntityState.Unchanged:
                    break;
                case Microsoft.EntityFrameworkCore.EntityState.Deleted:
                    Before = ((IReactiveDbObject)entry.Entity).RaiseDbEntityDeleting;
                    After = ((IReactiveDbObject)entry.Entity).RaiseDbEntityDeleted;
                    break;
                case Microsoft.EntityFrameworkCore.EntityState.Modified:
                    Before = ((IReactiveDbObject)entry.Entity).RaiseDbEntityUpdating;
                    After = ((IReactiveDbObject)entry.Entity).RaiseDbEntityUpdated;
                    break;
                case Microsoft.EntityFrameworkCore.EntityState.Added:
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