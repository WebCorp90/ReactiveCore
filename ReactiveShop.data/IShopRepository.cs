using ReactiveShop.Core;
using ReactiveShop.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactiveShop.data
{
    public interface IShopRepository<T>:IRepository<T> where T:BaseEntity
    {
        
    }
}
