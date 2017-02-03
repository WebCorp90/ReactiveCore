using ReactiveCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveDbCore
{
    public interface IRepository<T> where T : ReactiveObject
    {
        T Get<TKey>(TKey id);
        IQueryable<T> GetAll();
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete<TKey>(TKey key);
    }
}
