using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace ReactiveCore.Test
{
    [TestClass]
    public class TestReactiveCore
    {
        [TestMethod]
        public void TestMethod1()
        {
            BlogCore b = new BlogCore();

           b.Changed.Subscribe(e => {
                if (Debugger.IsAttached) Debugger.Break();
                Trace.WriteLine($"{e.PropertyName} has changed ");
            });

            b.Name = "toto";
            b.Category = "tiit";


        }
    }
}
