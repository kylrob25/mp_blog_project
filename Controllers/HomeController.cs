using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using KRoberts_Theatre_Blog.Models;

namespace KRoberts_Theatre_Blog.Controllers
{
    public class HomeController : Controller
    {
        private readonly BlogDatabaseContext _context = new BlogDatabaseContext();

        public ActionResult Index()
        {
            var posts = _context.Posts.Include(post => post.Category)
                .Include(post => post.User).OrderByDescending(post => post.PublishDate);

            ViewBag.Categories = _context.Categories.ToList();
            
            return View(posts.ToList());
        }

        [HttpPost]
        public ActionResult Index(string category)
        {
            if (category == null || category.IsEmpty() || !_context.Categories.Select(c => c.Name).Contains(category))
            {
                return RedirectToAction("Index");
            }

            var posts = _context.Posts.Include(post => post.Category).Include(post => post.User)
                .Where(post => post.Category.Name.Equals(category.Trim())).OrderByDescending(post => post.PublishDate);
            return View(posts.ToList());
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}