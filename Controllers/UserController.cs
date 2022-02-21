using KRoberts_Theatre_Blog.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace KRoberts_Theatre_Blog.Controllers
{
    public class UserController : Controller
    {
        private readonly BlogDatabaseContext _context = new BlogDatabaseContext();

        [Authorize(Roles = "Admin,Moderator")]
        [HttpGet]
        public ActionResult All()
        {
            return View(_context.Users.ToList());
        }

        [Authorize(Roles = "Admin,Moderator")]
        [HttpGet]
        public ActionResult Details(string id)
        {
            // Ensuring the id is not null
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = _context.Users.Find(id); // Grabbing the user from the table and ensuring its not null
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user); // Sending the page
        }

        [Authorize(Roles = "Admin,Moderator")]
        [HttpGet]
        public ActionResult Promote(string id)
        {
            // Ensuring the id is not null
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = _context.Users.Find(id); // Grabbing our user from the table and ensuring it is not null
            if (user == null)
            {
                return HttpNotFound();
            }

            if (user.Role == "Admin")
            {
                return RedirectToAction("Details", new { id = id });
            }

            return View(user); // Returning our view
        }

        [Authorize(Roles = "Admin,Moderator")]
        [HttpPost, ActionName("Promote")]
        [ValidateAntiForgeryToken]
        public ActionResult PromoteConfirm(string id)
        {
            var user = _context.Users.Find(id);
            if (!user.IsStaff)
            {
                user.UpdateRole("Moderator");
                _context.Entry(user).State = EntityState.Modified;
                _context.SaveChanges();
            }
            return RedirectToAction("Details", new {id = id});
        }

        [Authorize(Roles = "Admin,Moderator")]
        [HttpGet]
        public ActionResult Suspend(string id)
        {
            // Ensuring the id is not null
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = _context.Users.Find(id); // Grabbing our user from the table and ensuring it is not null
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user); // Returning our view
        }

        [Authorize(Roles = "Admin,Moderator")]
        [HttpPost, ActionName("Suspend")]
        [ValidateAntiForgeryToken]
        public ActionResult SuspendConfirm(string id)
        {
            var user = _context.Users.Find(id);
            user.Suspended = !user.Suspended;
            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("Details", new {id = id});
        }
    }
}