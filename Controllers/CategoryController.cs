using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using KRoberts_Theatre_Blog.Models;

namespace KRoberts_Theatre_Blog.Controllers
{
    public class CategoryController : Controller
    {
        private readonly BlogDatabaseContext _context = new BlogDatabaseContext();
        
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        public ActionResult View(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var posts = _context.Posts.Where(p => p.CategoryId == id)
                .Include(p => p.User).Include(p => p.Category).ToList();

            ViewBag.Categories = _context.Categories.ToList();
            
            return View(posts);
        }
    }
}