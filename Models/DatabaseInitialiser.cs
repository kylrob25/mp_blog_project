using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace KRoberts_Theatre_Blog.Models
{
    public class DatabaseInitialiser : DropCreateDatabaseAlways<BlogDatabaseContext>
    {
        protected override void Seed(BlogDatabaseContext context)
        {
            base.Seed(context);

            CreateRoles(context);

            CreateTestAdmin(context);
            CreateTestModerator(context);
            CreateTestMember(context);
        }

        private void CreateRoles(BlogDatabaseContext context)
        {
            if (!context.Users.Any())
            {
                // Grabbing our role manager
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

                // Default roles
                var roles = new[]
                {
                    "Admin",
                    "Moderator",
                    "Member"
                };

                // Ensuring our roles are created
                foreach (var role in roles)
                {
                    if (!roleManager.RoleExists(role))
                    {
                        roleManager.Create(new IdentityRole(role));
                    }
                }

                // Saving our roles
                context.SaveChanges();
            }
        }

        private void CreateTestAdmin(BlogDatabaseContext context)
        {
            // Grabbing our manager
            var userManager = new UserManager<User>(new UserStore<User>(context));

            // Checking if the user exists
            if (userManager.FindByName("admin@theatreblog.com") == null)
            {
                var admin = new User
                {
                    UserName = "admin@theatreblog.com",
                    Email = "admin@theatreblog.com",
                    FirstName = "Kyle",
                    LastName = "Roberts"
                };
                userManager.Create(admin, "8j3VRGqgyhQnVf6"); // Creating the user
                userManager.AddToRole(admin.Id, "Admin"); // Assigning the role
            }
        }

        private void CreateTestModerator(BlogDatabaseContext context)
        {
            // Grabbing our manager
            var userManager = new UserManager<User>(new UserStore<User>(context));

            // Checking if the user exists
            if (userManager.FindByName("moderator@theatreblog.com") == null)
            {
                var moderator = new User
                {
                    UserName = "moderator@theatreblog.com",
                    Email = "moderator@theatreblog.com",
                    FirstName = "Kyle",
                    LastName = "Roberts"
                };
                userManager.Create(moderator, "8j3VRGqgyhQnVf6"); // Creating the user
                userManager.AddToRole(moderator.Id, "Moderator"); // Assigning the role
            }
        }

        private void CreateTestMember(BlogDatabaseContext context)
        {
            var member = new User
            {
                UserName = "member@theatreblog.com",
                Email = "member@theatreblog.com",
                FirstName = "Kyle",
                LastName = "Roberts"
            };

            // Grabbing our manager
            var userManager = new UserManager<User>(new UserStore<User>(context));

            // Checking if the user exists
            if (userManager.FindByName("member@theatreblog.com") == null)
            {
                userManager.Create(member, "8j3VRGqgyhQnVf6"); // Creating the user
                userManager.AddToRole(member.Id, "Member"); // Assigning the role
            }

            var announcements = new Category() { Name = "Announcements" };
            var news = new Category() { Name = "News" };
            var movies = new Category() { Name = "Movies" };
            var theatre = new Category() { Name = "Theatre" };

            var categories = new[]
            {
                announcements,
                news,
                movies,
                theatre
            };

            // Adding to the table
            context.Categories.AddRange(categories);

            context.SaveChanges();

            var random = new Random();

            for (var i = 1; i < 10; i++)
            {
                var post = new Post()
                {
                    Title = "Post #" + i,
                    Category = categories[random.Next(0, categories.Length)],
                    Content = "Lorem Ipsum is simply dummy text of the printing and typesetting industry." +
                              " Lorem Ipsum has been the industry's standard dummy text ever since the 1500s," +
                              " when an unknown printer took a galley of type and scrambled it to make a type" +
                              " specimen book. It has survived not only five centuries, but also the leap into " +
                              "electronic typesetting, remaining essentially unchanged. It was popularised in the " +
                              "1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more " +
                              "recently with desktop publishing software like Aldus PageMaker including versions of " +
                              "Lorem Ipsum.",
                    CreationDate = DateTime.Now,
                    PublishDate = DateTime.Now,
                    LastEditDate = DateTime.Now,
                    Published = true,
                    User = member
                };

                context.Posts.Add(post);
                
                var comment = new Comment()
                {
                    Content = "Bad post " + i,
                    PublishDate = DateTime.Now,
                    User = member,
                    Post = post
                };
                var comments = new List<Comment> { comment };

                post.Comments = comments;
            }

            // Saving our changes
            context.SaveChanges();
        }
    }
}