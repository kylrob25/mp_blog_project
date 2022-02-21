using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace KRoberts_Theatre_Blog.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class User : IdentityUser
    {
        [Display(Name = "First Name")] public string FirstName { get; set; }
        [Display(Name = "Last Name")] public string LastName { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public bool Suspended { get; set; }

        public List<Post> Posts { get; set; }
        public List<Comment> Comments { get; set; }
        
        public bool IsStaff => Role != "Member";

        private ApplicationUserManager _userManager;

        public string Role
        {
            get
            {
                if (_userManager == null)
                {
                    _userManager = HttpContext.Current.GetOwinContext().Get<ApplicationUserManager>();
                }

                return _userManager.GetRoles(Id).Single();
            }
        }

        public void UpdateRole(string roleString)
        {
            if (_userManager == null)
            {
                _userManager = HttpContext.Current.GetOwinContext().Get<ApplicationUserManager>();
            }

            foreach (var role in _userManager.GetRoles(Id))
            {
                _userManager.RemoveFromRole(Id, role);
            }
                
            _userManager.AddToRole(Id, roleString);
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}