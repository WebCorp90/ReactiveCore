using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReactiveShop.Core.Domain.Catalog;
using Webcorp.unite;
using System.Diagnostics;

namespace ReactiveShopCore.Test
{
    [TestClass]
    public class ProductTest
    {
        [TestMethod]
        public void TestMass()
        {
            Product p = new Product();
            p.Changed.Subscribe(e => Trace.WriteLine($"{e.PropertyName} has changed"));
            p.MassLinear = MassLinear.KilogrammePerMeter * 0.282;
            p.Length = Length.Millimetre * 1200;

            Assert.IsFalse(p.MassAutoCalculated);
            Assert.IsNull(p.Mass);

            p.MassAutoCalculated = true;
            p.Length = Length.Millimetre * 1300;

            Assert.IsNotNull(p.Mass);
            Assert.AreEqual(p.Mass, p.MassLinear * p.Length);
        }
    }
}
