using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
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
            var entries = dbContext.ChangeTracker.Entries().ToList();
            var triggeredEntries = new List<EntityEntry>(entries.Count);
            while (entries.Any())
            {
                foreach (var entry in entries)
                {
                    if (typeof(IReactiveDbObject).IsAssignableFrom(entry.Entity.GetType()))
                    {
                        IReactiveDbObject entity = entry.Entity as IReactiveDbObject;
                        if (entry.State == Microsoft.EntityFrameworkCore.EntityState.Added)
                        {
                            entity.RaiseDbEntityAdded();

                        }
                    }
                    triggeredEntries.Add(entry);
                }
                var newEntries = dbContext.ChangeTracker.Entries().Except(triggeredEntries, EntityEntryComparer.Default);
                entries.Clear();
                entries.AddRange(newEntries);
            }
            return baseSaveChanges(acceptAllChangesOnSuccess);

        }
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
    }
}