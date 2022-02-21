using KRoberts_Theatre_Blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace KRoberts_Theatre_Blog.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        private readonly BlogDatabaseContext _context = new BlogDatabaseContext();

        // GET
        public ActionResult All()
        {
            return View(_context.Users.ToList());
        }

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

            return View(user); // Returning our view
        }

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
    }
}