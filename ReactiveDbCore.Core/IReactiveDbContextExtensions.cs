#if CORE
using Microsoft.EntityFrameworkCore;
#endif
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ReactiveHelpers;

namespace ReactiveDbCore
{
    public static  class IReactiveDbContextExtensions
    {
        /// <summary>
		/// Detaches all entities from the current object context
		/// </summary>
		/// <param name="unchangedEntitiesOnly">When <c>true</c>, only entities in unchanged state get detached.</param>
		/// <returns>The count of detached entities</returns>
		public static int DetachAll(this IReactiveDbContext ctx, bool unchangedEntitiesOnly = true)
        {
            return ctx.DetachEntities<ReactiveDbObject>(unchangedEntitiesOnly);
        }

        public static void DetachEntities<TEntity>(this IReactiveDbContext ctx, IEnumerable<TEntity> entities) where TEntity : ReactiveDbObject
        {
            Contract.Requires<ArgumentNullException>(ctx != null, nameof(ctx));
            entities.Each(x => ctx.DetachEntity(x));
        }

        /// <summary>
        /// Changes the object state to unchanged
        /// </summary>
        /// <typeparam name="TEntity">Type of entity</typeparam>
        /// <param name="ctx"></param>
        /// <param name="entity">The entity instance</param>
        /// <returns>true on success, false on failure</returns>
        public static bool SetToUnchanged<TEntity>(this IReactiveDbContext ctx, TEntity entity) where TEntity : ReactiveDbObject
        {
            try
            {
                ctx.ChangeState<TEntity>(entity, EntityState.Unchanged);
                return true;
            }
            catch (Exception ex)
            {
                ex.Dump();
                return false;
            }
        }

        public static IQueryable<TCollection> QueryForCollection<TEntity, TCollection>(
            this IReactiveDbContext ctx,
            TEntity entity,
            Expression<Func<TEntity, IEnumerable<TCollection>>> navigationProperty)
            where TEntity : ReactiveDbObject
            where TCollection : ReactiveDbObject
        {
            Contract.Requires<ArgumentNullException>(entity != null, nameof(entity));
            Contract.Requires<ArgumentNullException>(navigationProperty != null, nameof(navigationProperty));
            

            var dbContext = ctx as DbContext;
            if (dbContext == null)
            {
                throw new NotSupportedException("The IReactiveDbContext instance does not inherit from DbContext (EF)");
            }

            return dbContext.Entry(entity).Collection(navigationProperty).Query();
        }

        public static IQueryable<TProperty> QueryForReference<TEntity, TProperty>(
            this IReactiveDbContext ctx,
            TEntity entity,
            Expression<Func<TEntity, TProperty>> navigationProperty)
            where TEntity : ReactiveDbObject
            where TProperty : ReactiveDbObject
        {
            Contract.Requires<ArgumentNullException>(entity != null, nameof(entity));
            Contract.Requires<ArgumentNullException>(navigationProperty != null, nameof(navigationProperty));

            var dbContext = ctx as DbContext;
            if (dbContext == null)
            {
                throw new NotSupportedException("The IReactiveDbContext instance does not inherit from DbContext (EF)");
            }

            return dbContext.Entry(entity).Reference(navigationProperty).Query();
        }

        public static void LoadCollection<TEntity, TCollection>(
            this IReactiveDbContext ctx,
            TEntity entity,
            Expression<Func<TEntity, IEnumerable<TCollection>>> navigationProperty,
            bool force = false,
            Func<IQueryable<TCollection>, IQueryable<TCollection>> queryAction = null)
            where TEntity : ReactiveDbObject
            where TCollection : ReactiveDbObject
        {
            Contract.Requires<ArgumentNullException>(entity != null, nameof(entity));
            Contract.Requires<ArgumentNullException>(navigationProperty != null, nameof(navigationProperty));
            var dbContext = ctx as DbContext;
            if (dbContext == null)
            {
                throw new NotSupportedException("The IReactiveDbContext instance does not inherit from DbContext (EF)");
            }

            var entry = dbContext.Entry(entity);
            var collection = entry.Collection(navigationProperty);

            if (force)
            {
                collection.IsLoaded = false;
            }

            if (!collection.IsLoaded)
            {
                if (queryAction != null || ctx.ForceNoTracking)
                {
                    var query = !ctx.ForceNoTracking
                        ? collection.Query()
                        : collection.Query().AsNoTracking();

                    var myQuery = queryAction != null
                        ? queryAction(query)
                        : query;

                    collection.CurrentValue = myQuery.ToList();
                }
                else
                {
                    collection.Load();
                }

                collection.IsLoaded = true;
            }
        }

        public static void LoadReference<TEntity, TProperty>(
            this IReactiveDbContext ctx,
            TEntity entity,
            Expression<Func<TEntity, TProperty>> navigationProperty,
            bool force = false)
            where TEntity : ReactiveDbObject
            where TProperty : ReactiveDbObject
        {
            Contract.Requires<ArgumentNullException>(entity != null, nameof(entity));
            Contract.Requires<ArgumentNullException>(navigationProperty != null, nameof(navigationProperty));
            
            var dbContext = ctx as DbContext;
            if (dbContext == null)
            {
                throw new NotSupportedException("The IReactiveDbContext instance does not inherit from DbContext (EF)");
            }

            var entry = dbContext.Entry(entity);
            var reference = entry.Reference(navigationProperty);

            if (force)
            {
                reference.IsLoaded = false;
            }

            if (!reference.IsLoaded)
            {
                reference.Load();
                reference.IsLoaded = true;
            }
        }

        public static void AttachRange<TEntity>(this IReactiveDbContext ctx, IEnumerable<TEntity> entities) where TEntity : ReactiveDbObject
        {
            entities.Each(x => ctx.Attach(x));
        }
    }
}
