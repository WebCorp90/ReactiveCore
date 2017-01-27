using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveDbCore.Test
{
    public class BlogContext:ReactiveDbContext
    {
        public BlogContext():base()
        {

        }

        public BlogContext(DbContextOptions<BlogContext> options):base(options)
        {

        }
        public DbSet<Blog> Blogs { get; set; }
    }
}
