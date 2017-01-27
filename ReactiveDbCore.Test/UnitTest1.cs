using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ReactiveDbCore.Test
{
    [TestClass]
    public class BlogTest:BlogTestBase
    {
        [TestMethod]
        public void TestMethod1()
        {
            var options = new DbContextOptionsBuilder<BlogContext>()
                .UseInMemoryDatabase(databaseName: "Add_writes_to_database")
                .Options;
            // Run the test against one instance of the context
            using (var context = new BlogContext(options))
            {
                var adding = false;
                Assert.IsTrue(context.TriggersEnabled);
                Blog blog = new Blog();
                blog.Added.Subscribe(e =>
                {
                    if (Debugger.IsAttached) Debugger.Break();
                    Trace.WriteLine($"{nameof(e.Sender)} is added ");
                    adding = true;
                });
                blog.Changed.Subscribe(e => {
                    if (Debugger.IsAttached) Debugger.Break();
                    Trace.WriteLine($"{e.PropertyName} has changed ");
                });
                blog.Url = "www.google.fr";
                context.Blogs.Add(blog);
                context.SaveChanges();
                Assert.IsTrue(adding);
            }
        }
    }
}
