using Microsoft.AspNet.Identity.EntityFramework;

namespace KRoberts_Theatre_Blog.Models
{
    public class BlogDatabaseContext : IdentityDbContext<User>
    {
        public BlogDatabaseContext()
            : base("BlogConnection", throwIfV1Schema: false)
        {
        }

        public static BlogDatabaseContext Create()
        {
            return new BlogDatabaseContext();
        }
    }
}