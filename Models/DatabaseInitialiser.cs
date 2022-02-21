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
                var roles = new string[]
                {
                    "Admin",
                    "Moderator",
                    "Member"
                };

                // Ensuring our roles are created
                for (int i = 0; i < roles.Length; i++)
                {
                    var role = roles[i];
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

            // Adding to the table
            context.Categories.Add(news);
            context.Categories.Add(announcements);
            context.Categories.Add(new Category() { Name = "Movies" });
            context.Categories.Add(new Category() { Name = "Theatre" });

            context.SaveChanges();

            var post = new Post()
            {
                Title = "Test Post",
                Category = announcements,
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
                User = member
            };
            
            var post1 = new Post()
            {
                Title = "Test Post 1",
                Category = news,
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
                User = member
            };

            context.Posts.Add(post);
            context.Posts.Add(post1);

            context.SaveChanges();

            var comments = new List<Comment>();
            var comment = new Comment()
            {
                Content = "Bad post",
                PublishDate = DateTime.Now,
                User = member,
                Post = post
            };
            comments.Add(comment);

            post.Comments = comments;

            // Saving our changes
            context.SaveChanges();
        }
    }
}