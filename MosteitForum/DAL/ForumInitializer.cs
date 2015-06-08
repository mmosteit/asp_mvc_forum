using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MosteitForum.Models;

namespace MosteitForum.DAL
{
    public class ForumInitializer : System.Data.Entity.CreateDatabaseIfNotExists<ForumContext>
    {
        protected override void Seed(ForumContext context)
        {
            Thread FirstThread = new Thread { ThreadTitle = "First Thread", DatePosted = new DateTime(2015,1,1), UserName = "Michael" };
            context.Threads.Add(FirstThread);
            context.SaveChanges();

            Post ParentPost = new Post {Username = "Michael", ParentID = 0, DatePosted = new DateTime(2015,1,1), ThreadID = FirstThread.ThreadID, Deleted = false };
            context.Posts.Add(ParentPost);
            context.SaveChanges();

            Post FirstPost = new Post {Username = "Michael", ParentID = ParentPost.ThreadID, DatePosted = new DateTime(2015,1,1), ThreadID = FirstThread.ThreadID, Deleted = false, PostText = "First Post" };

            FirstThread.FirstPostID = ParentPost.PostID;

  
            context.Posts.Add(FirstPost);

            context.SaveChanges();

            Console.WriteLine("Just seeded database");
        }
    }
}