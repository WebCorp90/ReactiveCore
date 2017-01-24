using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace ReactiveCore.Test
{
    [TestClass]
    public class TestReactiveUI
    {
        [TestMethod]
        public void TestMethod1()
        {
            BlogUI b = new BlogUI();

            b.Changed.Subscribe(e => {
                if (Debugger.IsAttached) Debugger.Break();
                Trace.WriteLine( $"{e.PropertyName} has changed ");
            });

            b.Name = "toto";
            b.Name = "titi";
            b.Category = "tiit";


        }
    }
}
