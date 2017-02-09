#if CORE
using Microsoft.EntityFrameworkCore;
#else
using System.Data.Entity;
#endif
using ReactiveCore;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveDbCore
{

    public interface IUnitOfWork<T> where T:DbContext, IDisposable
    {
        T Context { get; }

        R GetRepository<R, O>() where R : IRepository<O> where O : ReactiveObject;

        Task Commit();

        Task Rollback();


    }
}
