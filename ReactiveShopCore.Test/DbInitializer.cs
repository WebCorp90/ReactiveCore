using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveShopCore.Test
{
    public class DbInitializer : DropCreateDatabaseAlways<ShopDataContext>
    {
        protected override void Seed(ShopDataContext context)
        {
            context.PrepareData();
        }
    }
}
