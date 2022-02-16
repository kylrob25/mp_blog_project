using System.Data.Entity;
using System.Linq;
using System.Net;
using KRoberts_Theatre_Blog.Models;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Web;
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
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Admin
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var totalUsers = context.Users.Count();
            var totalSuspended = context.Users.Count(user => user.Suspended);
            var totalMembers = Enumerable.Count(context.Users, user => user.Role == "Member");
            var totalStaff = totalUsers - totalMembers;

            int[] statistics =
             {
                totalUsers,
                totalStaff,
                totalMembers,
                totalSuspended
             };
            return View(statistics);
        }

        public ActionResult ViewUsers()
        {
            return View(context.Users.ToList());
        }

        public ActionResult ViewPosts()
        {
            var posts = context.Posts.ToList();
            foreach (var post in posts)
            {
                post.Category = context.Categories.Find(post.CategoryId);
                post.User = context.Users.Find(post.UserId);
            }
            return View(context.Posts.ToList());
        }

        public ActionResult ViewCategories()
        {
            return View(context.Categories.ToList());
        }

        public ActionResult DetailsUser(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = context.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        public ActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCategory([Bind(Include = "Id, Name")] Category category)
        {
            if (!ModelState.IsValid) return View(category);
            context.Categories.Add(category);
            context.SaveChanges();
            return RedirectToAction("ViewCategories");
        }

        public ActionResult EditCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var category = context.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }

            return View(category);
        }

        public ActionResult EditCategory([Bind(Include = "Id,Name")] Category category)
        {
            if (!ModelState.IsValid) return View(category);
            context.Entry(category).State = EntityState.Modified;
            context.SaveChanges();
            return RedirectToAction("ViewCategories");
        }

        public ActionResult DeleteCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var category = context.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }

            return View(category);
        }

        [HttpPost, ActionName("DeleteCategory")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCategoryConfirm(int id)
        {
            var category = context.Categories.Find(id);
            context.Categories.Remove(category);
            context.SaveChanges();
            return RedirectToAction("ViewCategories");
        }
    }
}