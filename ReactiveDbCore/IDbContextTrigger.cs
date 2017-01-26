using System;
using System.Threading;
using System.Threading.Tasks;

namespace ReactiveDbCore
{
    public static class IDbContextTrigger
    {
        public static int SaveChangesWithTriggers<TReactiveDbContext>(this TReactiveDbContext dbContext, Func<Boolean, Int32> baseSaveChanges, Boolean acceptAllChangesOnSuccess = true)
        {
        }
        public static async Task<int> SaveChangesWithTriggersAsync<TReactiveDbContext>(this TReactiveDbContext dbContext, Func<Boolean, CancellationToken, Task<Int32>> baseSaveChangesAsync, Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            
        }
        public static Task<Int32> SaveChangesWithTriggersAsync<TReactiveDbContext>(this TReactiveDbContext dbContext, Func<Boolean, CancellationToken, Task<Int32>> baseSaveChangesAsync, CancellationToken cancellationToken = default(CancellationToken))
        {
            return dbContext.SaveChangesWithTriggersAsync(baseSaveChangesAsync, true, cancellationToken);
        }
    }
}