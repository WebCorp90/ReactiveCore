using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ReactiveDbCore.Test
{
    [TestClass]
    public class BlogTestBase
    {
        private TestContext testContextInstance;

        private static DbContextOptions<BlogContext> dbOptions;
        protected BlogContext Context;
        /// <summary>
        ///Obtient ou définit le contexte de test qui fournit
        ///des informations sur la série de tests active, ainsi que ses fonctionnalités.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            dbOptions = new DbContextOptionsBuilder<BlogContext>()
                .UseInMemoryDatabase(databaseName: "Add_writes_to_database")
                .Options;
        }

        [TestInitialize()]
        public void MyTestInitialize()
        {
            dbOptions = new DbContextOptionsBuilder<BlogContext>()
                .UseInMemoryDatabase(databaseName: "Add_writes_to_database")
                .Options;
            Context = new BlogContext(dbOptions);
        }

        [TestCleanup()]
        public void MyTestCleanup()
        {
            Context.Dispose();
            GC.Collect();
        }

        protected void Add(Blog b)
        {
            Context.Blogs.Add(b);
        }

        protected void Delete(Blog b)
        {
            Context.Remove(b);
        }

        protected void Save()
        {
            Context.SaveChanges();
        }
    }
}