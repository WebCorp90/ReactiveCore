using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Reactive.Linq;
namespace ReactiveDbCore.Test
{
    [TestClass]
    public class BlogTest : BlogTestBase
    {
        [TestMethod]
        public void TestAddFlags()
        {

            var adding = false;
            var added = false;
            Assert.IsTrue(Context.TriggersEnabled);
            Blog blog = new Blog();
            blog.Adding.Subscribe(e =>
            {
                if (Debugger.IsAttached) Debugger.Break();
                Trace.WriteLine($"{nameof(e.Sender)} is adding ");
                adding = true;
            });
            blog.Added.Subscribe(e =>
            {
                if (Debugger.IsAttached) Debugger.Break();
                Trace.WriteLine($"{nameof(e.Sender)} is added ");
                added = true;
            });
            blog.Changed.Subscribe(e =>
            {
                if (Debugger.IsAttached) Debugger.Break();
                Trace.WriteLine($"{e.PropertyName} has changed ");
            });
            blog.Url = "www.google.fr";
            Add(blog);
            Save();
            Assert.IsTrue(adding);
            Assert.IsTrue(added);

        }


        [TestMethod]
        [ExpectedException(typeof(DbUpdateException))]
        public void TestDeleteFlagsWithException()
        {
            var deleting = false;
            Blog b = new Test.Blog();
            b.Url = "www.free.Fr";
            Delete(b);
            Save();
        }

        [TestMethod]
        [ExpectedException(typeof(DbUpdateException))]
        public void TestDeleteFlagsWithSubscriber()
        {
            var deleting = false;
            Blog b = new Test.Blog();
            b.Error.Subscribe(e =>
            {
                if (Debugger.IsAttached) Debugger.Break();
                Trace.WriteLine(e.Exception.Message);
            });
            b.Url = "www.free.Fr";
            Delete(b);
            Save();
        }

        [TestMethod]
        [ExpectedException(typeof(DbUpdateException))]
        public void TestDeleteFlagsWithSubscriberAndContextError()
        {
            Context.Error.Subscribe(e =>
            {
                if (Debugger.IsAttached) Debugger.Break();
                Trace.WriteLine(e.Exception.Message);
            });
            Blog b = new Test.Blog();
            b.Error.Subscribe(e =>
            {
                if (Debugger.IsAttached) Debugger.Break();
                Trace.WriteLine(e.Exception.Message);
            });
            b.Url = "www.free.Fr";
            Delete(b);
            Save();
        }

        [TestMethod]
        [ExpectedException(typeof(DbUpdateException))]
        public void TestDeleteFlagsWithSubscriberAndContextErrorRequired()
        {
            Context.Error.Where(e => e.Exception is ValidationEntityException).Subscribe(e => {
                if (Debugger.IsAttached) Debugger.Break();
            });
            Context.Error.Subscribe(e =>
            {
                if (Debugger.IsAttached) Debugger.Break();
                Trace.WriteLine(e.Exception.Message);
            });
            Blog b = new Blog();
            Add(b);
            Blog b0 = new Blog();
            Add(b0);
            b.Error.Subscribe(e =>
            {
                if (Debugger.IsAttached) Debugger.Break();
                Trace.WriteLine(e.Exception.Message);
            });
            b.Added.Subscribe(e =>
            {
                Trace.WriteLine($"{b.Url} added");
            });
            b.Url = "www.free.Fr";
            b0.Category = "web";
            //Delete(b);
            Save();
        }

    }
}
