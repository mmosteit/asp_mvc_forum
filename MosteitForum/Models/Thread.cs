using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MosteitForum.Models
{
    public class Thread
    {
        [Key]
        public int ThreadID       {get; set;}
        public String ThreadTitle { get; set; }
        public int? FirstPostID   { get; set; } /* Used to build the post tree */
        public DateTime DatePosted {get; set;}
        public String UserName { get; set; }

    }

    public class ThreadDBContext : DbContext
    {
        public DbSet<Thread> Threads { get; set; }
    }
}