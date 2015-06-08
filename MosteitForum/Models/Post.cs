using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MosteitForum.Models
{
    public class Post
    {
  
        [Key]
        public int PostID {get; set; }
	    public String Username{get; set;} 
	    public int? ParentID {get; set;}
        public String PostText { get; set; }
	    public DateTime DatePosted {get; set;}
        public int? ThreadID {get; set;}    /* The thread that this post belongs to */
        
        public bool Deleted {get; set;}     /* Used so that moderators can delete offensive posts */

        // Navigation properties

        public virtual Post parent { get; set; }

        // All posts responding to this post
        public virtual ICollection<Post> Children {get; set;}

    }

    public class PostDBContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }
    }

}