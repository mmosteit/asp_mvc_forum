using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MosteitForum.DAL;
using MosteitForum.Models;

namespace MosteitForum.Controllers
{
    public class PostsController : Controller
    {
        private ForumContext db = new ForumContext();

        // GET: Posts
        public ActionResult Index()
        {
            return View(db.Posts.ToList());
        }

        [HttpPost]
        public ActionResult NewPost(string username, string post_text, int parent_id, int thread_id)
        {

            if(post_text.Trim() == "" || username.Trim() == "" )
            {
                return RedirectToAction("NewPostError", "Posts", new { thread_id  });
            }
            
            DateTime new_date_posted = DateTime.Now;


            Post post;

            // Figure out the parameters
            post = new Post();

            post.DatePosted = new_date_posted;
            post.Username   = username;
            post.PostText   = post_text;
            post.ThreadID   = thread_id;
            post.ParentID   = parent_id;

            // Insert the post into the database
            db.Posts.Add(post);
            db.SaveChanges();

            // If there was an error, redirect to an error page

            // Otherwise, redirect to the same thread
            return RedirectToAction("ViewThread","Threads", new  {id = thread_id });
        }

        public ActionResult NewPostError(int thread_id)
        {
            String title = db.Threads.First(x => x.ThreadID == thread_id).ThreadTitle;
            ViewBag.title = title;
            ViewBag.ThreadId = thread_id;
            return View();
        }

        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }



        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PostID,Username,ParentID,PostText,DatePosted,ThreadID,Deleted")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(post);
        }

        // GET: Posts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PostID,Username,ParentID,PostText,DatePosted,ThreadID,Deleted")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(post);
        }

        // POST: Posts/Delete/5

        public ActionResult Delete(int? post_id)
        {
            if (post_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(post_id);
            
            if (post == null)
            {
                return HttpNotFound();
            }
            post.Deleted = true;
            db.SaveChanges();

            return RedirectToAction("ViewThread", "Threads", new { id = post.ThreadID });
            
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
