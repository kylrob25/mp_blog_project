using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KRoberts_Theatre_Blog.Models;
using KRoberts_Theatre_Blog.Models.ViewModels;
using Microsoft.AspNet.Identity;

namespace KRoberts_Theatre_Blog.Controllers
{
    public class PostController : Controller
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

            var post = _context.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            
            post.Category = _context.Categories.Find(post.CategoryId);
            post.User = _context.Users.Find(post.UserId);
            post.Comments = _context.Comments.Where(comment => comment.PostId == post.Id)
                .Include(comment => comment.User).OrderByDescending(comment => comment.PublishDate).ToList();
            
            return View(post);
        }

        [HttpGet]
        public ActionResult CreateComment(int? id)
        {
            var userId = User.Identity.GetUserId();
            var user = _context.Users.Find(userId);
            if (user == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // If the user suspended we do not allow them to make a comment
            if (user.Suspended)
            {
                return RedirectToAction("View", new {id = id});
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var post = _context.Posts.Where(p => p.Id == id).Include(p => p.User).Single();
            if (post == null)
            {
                return HttpNotFound();
            }

            var createComment = new CreateCommentViewModel()
            {
                PostId = post.Id,
                UserId = post.UserId
            };
            
            return View(createComment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateComment(int id, CreateCommentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var post = _context.Posts.Find(id);
                
                var comment = new Comment()
                {
                    Content = model.Content,
                    PostId = post.Id,
                    PublishDate = DateTime.Now,
                    UserId = User.Identity.GetUserId()
                };
                
                _context.Comments.Add(comment);
                _context.SaveChanges();

                return RedirectToAction("View", "Post", new { id = id });
            }

            return View(model);
        }
        
        public ActionResult DeleteComment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var comment = _context.Comments.Where(c => c.Id == id).Include(c => c.User).Single();
            if (comment == null)
            {
                return HttpNotFound();
            }

            return View(comment);
        }

        [HttpPost, ActionName("DeleteComment")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCommentConfirm(int id)
        {
            var comment = _context.Comments.Find(id);
            var postId = comment.PostId;
            _context.Comments.Remove(comment);
            _context.SaveChanges();
            return RedirectToAction("View", new { id = postId});
        }
    }
}