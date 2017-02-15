#if CORE
using Microsoft.EntityFrameworkCore;
#else
using System.Data.Entity;
#endif
using ReactiveCore;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections.Generic;

namespace ReactiveDbCore
{
    public class Repository<TKey,T> : IRepository<TKey,T> where T : ReactiveDbObject
    {

        //[Index("IX_FirstAndSecond", 2, IsUnique = true)]


        static Repository()
        {
            Key = ObjectMixins.GetKey<T>();
            Indexes=ObjectMixins.GetIndexes<T>();

        }
        private ReactiveDbContext _context;
        private DbSet<T> _dbset;

        public Repository(ReactiveDbContext context)
        {

            this.Context = context;
            this.DbSet = context.Set<T>();
        }

        public ReactiveDbContext Context { get { return _context; } private set { Contract.Requires(value != null); _context = value; } }
        protected DbSet<T> DbSet { get { return _dbset; } private set { Contract.Requires(value != null); _dbset = value; } }

        public static PropertyInfo Key { get; private set; }
        public static Dictionary<string, Indexes> Indexes { get; private set; }

        public void Add(T entity)
        {
            DbSet.Add(entity);
        }

        public async Task AddAsync(T entity)
        {
#if CORE
            await DbSet.AddAsync(entity);
#else       
            await Task.Run(() =>  Add(entity));
#endif
        }

        public void Delete(T entity)
        {
            if (entity == null) return;
            DbSet.Remove(entity);
        }

        public void Delete(TKey key)
        {
            Contract.Requires(key != null);
            Delete(Get(key));
        }

        public T Get(TKey key)
        {
            Contract.Requires(key != null);
            return DbSet.SingleOrDefault(r => Key.GetValue(r).Equals(key));
        }

        public Task<T> GetAsync(TKey key)
        {
            Contract.Requires(key != null);
            return DbSet.SingleOrDefaultAsync(r => Key.GetValue(r).Equals(key));
        }


        public IQueryable<T> GetAll()
        {
            Contract.Ensures(Contract.Result<IQueryable<T>>() != null);
            return DbSet;
        }

        public void Update(T entity)
        {
            Save();
        }

        public async Task<int> UpdateAsync()
        {
            return await SaveAsync();
        }

        protected virtual int Save()
        {
            return Context.SaveChanges();
        }

        protected virtual async Task<int> SaveAsync()
        {
            
            return await Context.SaveChangesAsync();
        }
    }
}
