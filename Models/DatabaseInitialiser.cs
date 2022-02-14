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
                var roles = new string[] {
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
                var admin = new Staff
                {
                    UserName = "admin@theatreblog.com",
                    Email = "admin@theatreblog.com",
                    FirstName = "Kyle",
                    LastName = "Roberts",
                    StaffType = StaffType.Admin
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
                var moderator = new Staff
                {
                    UserName = "moderator@theatreblog.com",
                    Email = "moderator@theatreblog.com",
                    FirstName = "Kyle",
                    LastName = "Roberts",
                    StaffType = StaffType.Moderator
                };
                userManager.Create(moderator, "8j3VRGqgyhQnVf6"); // Creating the user
                userManager.AddToRole(moderator.Id, "Moderator"); // Assigning the role
            }
        }

        private void CreateTestMember(BlogDatabaseContext context)
        {
            // Grabbing our manager
            var userManager = new UserManager<User>(new UserStore<User>(context));

            // Checking if the user exists
            if (userManager.FindByName("member@theatreblog.com") == null)
            {
                var member = new Staff
                {
                    UserName = "member@theatreblog.com",
                    Email = "member@theatreblog.com",
                    FirstName = "Kyle",
                    LastName = "Roberts",
                    StaffType = StaffType.Moderator
                };
                userManager.Create(member, "8j3VRGqgyhQnVf6"); // Creating the user
                userManager.AddToRole(member.Id, "Member"); // Assigning the role
            }
        }
    }
}