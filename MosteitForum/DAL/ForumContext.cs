using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

using MosteitForum.Models;

namespace MosteitForum.DAL
{
    public class ForumContext : DbContext
    {

        public ForumContext(): base("ForumContext")
        {
        }

        public DbSet<Post> Posts {get; set;}
        public DbSet<Thread> Threads { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

    }
}