using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MosteitForum.DAL;
using MosteitForum.Models;

namespace MosteitForum.Controllers
{
    public class ThreadsController : Controller
    {
        private ForumContext db = new ForumContext();

        // GET: Threads
        public ActionResult Index()
        {

            //return View(db.Threads.ToList());
            return View(db.Threads);
        }

        public void BuildTree(Post post)
        {

            // We are not dealing with the root node
            if (post.PostText != null)
            {
                post.parent = db.Posts.First(X => X.PostID == (int)(post.ParentID));
            }

            // Get the content any children.
            post.Children = new List<Post>();
            var AllChildren = db.Posts.Where(x => x.ParentID == post.PostID).OrderBy(x => x.DatePosted).ToList();
            Debug.WriteLine("Number of children = " + AllChildren.Count);
            foreach (Post postIter in AllChildren)
            {
                Debug.WriteLine("now adding child with post id of " + postIter.PostID + " to post " + post.PostID);
                BuildTree(postIter);
                post.Children.Add(postIter);
            }
            Debug.WriteLine("Number of children is now " + post.Children.Count + " for Post " + post.PostID);

        }

        public ActionResult ViewThread(int? id)
        {

            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "No id specified");
            }

            Thread thread = db.Threads.First(x => x.ThreadID == id);

            int FirstPostID = (int)thread.FirstPostID;

            ViewBag.title = thread.ThreadTitle;
            ViewBag.thread_id = id;

            Post rootPost = db.Posts.First(X => X.PostID == (int)(thread.FirstPostID));
            Debug.WriteLine("RootPOst.post id = " + rootPost.PostID);
            Debug.WriteLine("Now in ViewThread, rootPost.Children.Count = " + rootPost.Children.Count);
            BuildTree(rootPost);
            Debug.WriteLine("Just Built tree. rootPost.Children.Count = " + rootPost.Children.Count);


            Debug.WriteLine("Post text is " + rootPost.Children.First().PostText);
            return View(rootPost);

        }




        // POST: Threads/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string title, string UserName, string post_text)
        {
            if (title == null || title == "" || UserName == null || UserName == "" || post_text == null || post_text == "")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "One or more of the required POST parameters is null");
            }

            // Add the first thread
            Thread thread = new Thread();
            thread.ThreadTitle = title;
            thread.UserName = UserName;
            thread.DatePosted = DateTime.Now;
            db.Threads.Add(thread);
            db.SaveChanges();

            // Create the dummy root post
            Post root = new Post();
            root.Username = UserName;
            root.DatePosted = thread.DatePosted;
            root.ThreadID = thread.ThreadID;

            db.Posts.Add(root);
            db.SaveChanges();

            thread.FirstPostID = root.PostID;

            Post post = new Post();

            // Create the actual first post.
            post.DatePosted = thread.DatePosted;
            post.PostText = post_text;
            post.Username = UserName;
            post.ParentID = root.PostID;
            post.ThreadID = thread.ThreadID;

            db.Posts.Add(post);
            db.SaveChanges();

            return RedirectToAction("Index");

        }



        // GET: Threads/Delete/5
        [HttpPost]
        public ActionResult Delete(int? thread_id)
        {
            if (thread_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Thread thread = db.Threads.Find(thread_id);
            if (thread == null)
            {
                return HttpNotFound();
            }

            db.Threads.Remove(thread);

            // Remove all of the posts associated with this thread.
            var Posts = db.Posts.Where(x => x.ThreadID == thread_id);

            foreach (var post in Posts)
            {
                db.Posts.Remove(post);
            }
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
