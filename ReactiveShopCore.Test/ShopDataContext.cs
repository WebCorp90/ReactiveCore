using ReactiveShop.Core.Domain.Catalog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveShopCore.Test
{
    public class ShopDataContext : DbContext
    {
        public ShopDataContext() : base()
        {
            PrepareData();
        }
        public ShopDataContext(string name) : base(name)
        {

        }
        public DbSet<Product> Products { get; set; }


        public bool PrepareData()
        {

            for (int i = 1; i <= 20; i++)
                this.Products.Add(new Product() { Societe = "001", Code = $"code{i}", FullDescription = $"FullDesc{i}", Complementary = $"Comp{i}", CreatedBy = "jc" });

            try
            {
                this.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                ex.EntityValidationErrors.ToList().ForEach(e => e.ValidationErrors.ToList().ForEach(ee => Trace.WriteLine($"{ee.PropertyName}:{ ee.ErrorMessage}")));
                return false;
            }
            return true;


        }
    }
}
