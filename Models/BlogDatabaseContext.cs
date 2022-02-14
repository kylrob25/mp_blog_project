using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace KRoberts_Theatre_Blog.Models
{
    public class BlogDatabaseContext : IdentityDbContext<User>
    {
        public BlogDatabaseContext()
            : base("BlogConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer(new DatabaseInitialiser());
        }

        public static BlogDatabaseContext Create()
        {
            return new BlogDatabaseContext();
        }
    }
}