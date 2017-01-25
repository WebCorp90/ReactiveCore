using Microsoft.EntityFrameworkCore;
using ReactiveCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace ReactiveDbCore
{
    public class Repository<T> : IRepository<T> where T : ReactiveObject
    {

        //[Index("IX_FirstAndSecond", 2, IsUnique = true)]



        static Repository()
        {
            Keys = ObjectMixins.GetKeys<T>();
        }
        private ReactiveDbContext _context;
        private DbSet<T> _dbset;

        public Repository(ReactiveDbContext context)
        {
           
            this.Context = context;
            this.DbSet = context.Set<T>();
        }

        public ReactiveDbContext Context { get { return _context; } private set { Contract.Requires(value != null);  _context = value; } }
        protected DbSet<T> DbSet { get { return _dbset; } private set { Contract.Requires(value != null); _dbset = value; } }

        public static List<PropertyInfo> Keys { get; private set; }

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
            DbSet.Remove(entity);
        }

        public void Delete<TKey>(TKey key)
        {
            throw new NotImplementedException();
        }

        public T Get<TKey>(TKey id)
        {
            DbSet.Single(r=>r.)
        }

        public IQueryable<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
