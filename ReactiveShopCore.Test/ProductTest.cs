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
        public ShopDataContext Shop
        {
            get
            {
                string name = "ReactiveShopDatabase";
                //string name = "";
                if (name != string.Empty)
                {
                    Database.SetInitializer<ShopDataContext>(new DbInitializer());
                    return new ShopDataContext(name);
                }
                return new ShopDataContext();
            }
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
            Assert.AreEqual(p.Mass, Mass.Kilogram * 0);

            p.Material = new Material() { Code = "1.052", Density = Density.KilogramPerCubicMetre * 8000 };


            Assert.IsNotNull(p.Mass);
            Assert.AreEqual((int)p.Mass.Value, 36);
        }

        [TestMethod]
        public void TestCout()
        {


            using (var ctx = Shop)
            {
                var p = ctx.Products.Where(pp => pp.Code == "code1").FirstOrDefault();
                Assert.IsNotNull(p);
                Assert.IsTrue(p.CoutMainOeuvre.Value == 0);
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



    }
}
