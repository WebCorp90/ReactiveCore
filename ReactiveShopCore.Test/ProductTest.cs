using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReactiveShop.Core.Domain.Catalog;
using Webcorp.unite;
using System.Diagnostics;
using System.Data.Entity;
using System.Linq;
using System.Data.Entity.Validation;

namespace ReactiveShopCore.Test
{
    [TestClass]
    public class ProductTest
    {
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            Effort.Provider.EffortProviderConfiguration.RegisterProvider();
        }

        [TestInitialize]
        public void MyTestInitialize()
        {
            EffortProviderFactory.ResetDb();
        }


        [TestMethod]
        public void MyTestMethod()
        {
            var p = new Product();
            p.CoutMainOeuvre = Currency.Euro * 150;
            Assert.AreEqual(p._CoutMo, "150 euro");

            p._CoutMo = "125 euro";
            Assert.AreEqual(p.CoutMainOeuvre, Currency.Euro * 125);
        }
        [TestMethod]
        public void TestProductMass_MassLinear()
        {
            Product p = new Product();
            p.Changed.Subscribe(e => Trace.WriteLine($"{e.PropertyName} has changed"));
            p.MassLinear = MassLinear.KilogrammePerMeter * 0.282;
            p.Length = Length.Millimetre * 1200;

            Assert.IsFalse(p.MassAutoCalculated);
            Assert.IsNotNull(p.Mass);

            p.MassAutoCalculated = true;
            p.Length = Length.Millimetre * 1300;

            Assert.IsNotNull(p.Mass);
            Assert.AreEqual(p.Mass, p.MassLinear * p.Length);
        }


        [TestMethod]
        public void TestProductMass_MassDimensions()
        {
            Product p = new Product();
            p.Changed.Subscribe(e => Trace.WriteLine($"{e.PropertyName} has changed"));
            p.MassAutoCalculated = true;
            p.Length = Length.Millimetre * 3000;
            p.Width = Length.Metre * 1;
            p.Thickness = Length.Millimetre * 1.5;
            Assert.IsNotNull(p.Mass);
            Assert.AreEqual(p.Mass,Mass.Kilogram* 0);

            p.Material = new Material() { Code = "1.052", Density = Density.KilogramPerCubicMetre * 8000 };


            Assert.IsNotNull(p.Mass);
            Assert.AreEqual((int)p.Mass.Value, 36);
        }

        [TestMethod]
        public void TestCout()
        {
            Assert.IsTrue( PrepareData());
            using (var ctx=new ShopDataContext())
            {
                var p=ctx.Products.Where(pp=>pp.Code=="code1").FirstOrDefault();
                p.CoutMainOeuvre = Currency.Euro * 15;

                Assert.AreEqual(p.CoutTotal, p.CoutMainOeuvre);

                p.CoutMainOeuvre = Currency.FrancSuisse * 10;
                Assert.AreEqual(p.CoutTotal, p.CoutMainOeuvre);

                p.CoutMainOeuvre = Currency.Euro * 15;
                ctx.SaveChanges();

                var pc = ctx.Products.Where(pp => pp.Code == "code1").FirstOrDefault();
                Assert.AreEqual(p.CoutMainOeuvre.Value, 15);
            }
            
           
        }


        private bool PrepareData()
        {
            using (var ctx = new ShopDataContext())
            {
                for (int i = 1; i <= 20; i++)
                    ctx.Products.Add(new Product() { Societe = "001", Code = $"code{i}", FullDescription = $"FullDesc{i}",Complementary=$"Comp{i}" ,CreatedBy="jc"});



                try
                {
                    ctx.SaveChanges();
                }catch(DbEntityValidationException ex)
                {
                    ex.EntityValidationErrors.ToList().ForEach(e => e.ValidationErrors.ToList().ForEach(ee => Trace.WriteLine($"{ee.PropertyName}:{ ee.ErrorMessage}")));
                    return false;
                }
                return true;
                
            }
        }
    }
}
