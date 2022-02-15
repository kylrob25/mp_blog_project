using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace KRoberts_Theatre_Blog.Models
{
    public class BlogDatabaseContext : IdentityDbContext<User>
    {
        
        public DbSet<Post> Posts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        
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