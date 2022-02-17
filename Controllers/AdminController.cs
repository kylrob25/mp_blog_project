using System;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using KRoberts_Theatre_Blog.Models;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Web;
using KRoberts_Theatre_Blog.Models.ViewModels;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace KRoberts_Theatre_Blog.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationUserManager _userManager;
        private BlogDatabaseContext context = new BlogDatabaseContext();

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        // GET: Admin
        [Authorize(Roles = "Admin,Moderator")]
        public ActionResult Index()
        {
            var totalUsers = context.Users.Count(); // Calculating the total users
            var totalSuspended = context.Users.Count(user => user.Suspended); // Calculating suspended users
            var totalMembers = Enumerable.Count(context.Users, user => user.Role == "Member"); // Calculating members
            var totalStaff = totalUsers - totalMembers; // Calculating staff
            var totalPosts = context.Posts.Count();
            var totalCategories = context.Categories.Count();

            int[] statistics =
            {
                totalUsers,
                totalStaff,
                totalMembers,
                totalSuspended,
                totalPosts,
                totalCategories
            };
            return View(statistics); // Sending the page
        }

        public ActionResult ViewUsers()
        {
            return View(context.Users.ToList()); // Sending the page
        }

        public ActionResult DetailsUser(string id)
        {
            // Ensuring the id is not null
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = context.Users.Find(id); // Grabbing the user from the table and ensuring its not null
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user); // Sending the page
        }

        public ActionResult PromoteUser(string id)
        {
            // Ensuring the id is not null
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = context.Users.Find(id); // Grabbing our user from the table and ensuring it is not null
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user); // Returning our view
        }

        public ActionResult ViewPosts()
        {
            var posts = context.Posts.ToList(); // Grabbing our posts from the table
            foreach (var post in posts)
            {
                post.Category = context.Categories.Find(post.CategoryId); // Setting the category
                post.User = context.Users.Find(post.UserId); // Setting the author
            }

            return View(context.Posts.ToList()); // Sending the page
        }

        public ActionResult CreatePost()
        {
            ViewBag.CategoryId = new SelectList(context.Categories, "CategoryId", "Name");
            return View();
        }

        [HttpGet]
        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var post = context.Posts.Include(p => p.Category).SingleOrDefault(p => p.Id == id);
            if (post == null)
            {
                return HttpNotFound();
            }

            var editPost = new EditPostViewModel()
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                Published = post.Published,
                CategoryId = post.CategoryId
            };

            ViewBag.CategoryId = new SelectList(context.Categories, "CategoryId", "Name", post.CategoryId);

            return View(editPost);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int id, [Bind(Include ="Title,Content,CategoryId,Published")] EditPostViewModel model)
        {
            if (ModelState.IsValid)
            {
                var post = context.Posts.Find(id);
                post.Title = model.Title;
                post.Content = model.Content;
                post.CategoryId = model.CategoryId;
                post.Published = model.Published;
                post.LastEditDate = DateTime.Now;

                context.Entry(post).State = EntityState.Modified;
            
                context.SaveChanges();

                return RedirectToAction("ViewPosts");
            }

            ViewBag.CategoryId = new SelectList(context.Categories, "CategoryId", "Name", model.CategoryId);

            return View(model);
        }

        public ActionResult DeletePost(int? id)
        {
            // Ensuring the id is not null
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var post = context.Posts.Find(id); // Grabbing our post from the table and ensuring it is not null
            if (post == null)
            {
                return HttpNotFound();
            }

            return View(post); // Returning our view
        }

        [HttpPost, ActionName("DeletePost")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePostConfirm(int id)
        {
            var post = context.Posts.Find(id); // Grabbing our post from the table
            context.Posts.Remove(post); // Removing the post from the table
            context.SaveChanges(); // Saving our changes
            return RedirectToAction("ViewPosts"); // Redirecting to the posts page
        }

        public ActionResult ViewCategories()
        {
            return View(context.Categories.ToList()); // Sending the page
        }

        public ActionResult CreateCategory()
        {
            return View(); // Sending the page
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCategory([Bind(Include = "CategoryId, Name")] Category category)
        {
            if (!ModelState.IsValid) return View(category);
            context.Categories.Add(category); // Adding the category to our table
            context.SaveChanges(); // Saving our changes
            return RedirectToAction("ViewCategories"); // Redirecting to the categories page
        }

        public ActionResult EditCategory(int? id)
        {
            // Ensuring the id is not null
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var category =
                context.Categories.Find(id); // Grabbing our category from the table and ensuring its not null
            if (category == null)
            {
                return HttpNotFound();
            }

            return View(category); // Sending the page
        }

        public ActionResult EditCategory([Bind(Include = "CategoryId,Name")] Category category)
        {
            if (!ModelState.IsValid) return View(category);
            context.Entry(category).State = EntityState.Modified; // Updating our category
            context.SaveChanges(); // Saving our changes
            return RedirectToAction("ViewCategories"); // Redirecting to the categories page
        }

        public ActionResult DeleteCategory(int? id)
        {
            // Ensuring the id is not null
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var category =
                context.Categories.Find(id); // Grabbing the category from the table and ensuring its not null
            if (category == null)
            {
                return HttpNotFound();
            }

            return View(category); // Sending the page
        }

        [HttpPost, ActionName("DeleteCategory")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCategoryConfirm(int id)
        {
            var category = context.Categories.Find(id); // Grabbing the category from the table
            context.Categories.Remove(category); // Removing the category from the table
            context.SaveChanges(); // Saving our changes
            return RedirectToAction("ViewCategories"); // Redirecting to the categories page
        }
    }
}