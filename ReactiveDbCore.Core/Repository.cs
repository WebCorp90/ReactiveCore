using Microsoft.EntityFrameworkCore;
using ReactiveCore;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;

namespace ReactiveDbCore
{
    public class Repository<T> : IRepository<T> where T : ReactiveObject
    {

        //[Index("IX_FirstAndSecond", 2, IsUnique = true)]
        



        static Repository()
        {
            Key = ObjectMixins.GetKey<T>();
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

        public void Add(T entity)
        {
            DbSet.Add(entity);
        }

        public async Task AddAsync(T entity)
        {
            await DbSet.AddAsync(entity);
        }

        public void Delete(T entity)
        {
            if (entity == null) return;
            DbSet.Remove(entity);
        }

        public void Delete<TKey>(TKey key)
        {
            Contract.Requires(key != null);
            Delete(Get(key));
        }

        public T Get<TKey>(TKey key)
        {
            Contract.Requires(key != null);
            return DbSet.SingleOrDefault(r => Key.GetValue(r).Equals(key));
        }

        public Task< T> GetAsync<TKey>(TKey key)
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

        public async Task UpdateAsync()
        {
            await SaveAsync();
        }

        protected virtual void Save()
        {
            Context.SaveChanges();
        }

        protected virtual async Task SaveAsync()
        {
            
            await Context.SaveChangesAsync();
        }
    }
}
