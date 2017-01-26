using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveCore;

namespace ReactiveDbCore
{
   /* public class UnitOfWork<T> : IUnitOfWork<T> where T : DbContext
    {
        public UnitOfWork(T context)
        {
            this.Context = context;
        }
        public T Context
        {
            get;private set;
        }

        public async Task Commit()
        {
            try
            {
                await Context.SaveChangesAsync();
            }
            catch (Exception)
            {
                await Rollback();
            }
        }

        public R GetRepository<R, O>()
            where R : IRepository<O>
            where O : ReactiveObject
        {
            
        }

        public async Task Rollback()
        {
            await Task.Run(() => {
                Context
                   .ChangeTracker
                   .Entries()
                   .ToList()
                   .ForEach(x => x.Reload());
            });
           
        }
    }*/
}
