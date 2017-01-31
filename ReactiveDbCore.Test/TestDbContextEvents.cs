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
            //var deleting = false;
            Blog b = new Test.Blog();
            b.Url = "www.free.Fr";
            Delete(b);
            Save();
        }

        [TestMethod]
        [ExpectedException(typeof(DbUpdateException))]
        public void TestDeleteFlagsWithSubscriber()
        {
            //var deleting = false;
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
        public void TestDeleteFlagsWithSubscriberAndContextErrorRequired()
        {
            bool contextValidationError = false,contextError=false;
            bool blog_B_added =false, blog_B_error = false, blog_B_validationError = false; ;
            Context.ValidationError.Subscribe(e => {
                if (Debugger.IsAttached) Debugger.Break();
                Trace.WriteLine("There are several validation errors");
                contextValidationError = true;
            });
            Context.Error.Subscribe(e =>
            {
                if (Debugger.IsAttached) Debugger.Break();
                Trace.WriteLine(e.Exception.Message);
                contextError = true;
            });
            Blog b = new Blog();
            Add(b);
            Blog b0 = new Blog();
            Add(b0);
            b.Error.Subscribe(e =>
            {
                if (Debugger.IsAttached) Debugger.Break();
                Trace.WriteLine(e.Exception.Message);
                blog_B_error = true;
            });
            b.Added.Subscribe(e =>
            {
                blog_B_added = true;
                Trace.WriteLine($"{b.Url} added");
            });
            b.ValidationError.Subscribe(e =>
            {
                if (Debugger.IsAttached) Debugger.Break();
                Trace.WriteLine(e.Error.Exception.Message);
                blog_B_validationError = true;
            });
            b.Url = "www.free.Fr";
            b0.Category = "web";
            //Delete(b);
            Save();

            Assert.IsTrue(contextValidationError);
            Assert.IsFalse(contextError);
            Assert.IsTrue(blog_B_validationError);
            Assert.IsFalse(blog_B_added);
            Assert.IsFalse(blog_B_error);
        }

    }
}
