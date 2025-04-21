using BhaskarBlogApp.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace BhaskarBlogApp.Data
{
    public class BloggieDbContext : DbContext
    {
        public BloggieDbContext(DbContextOptions<BloggieDbContext> options) : base(options)
        {
        }

        public DbSet<BlogPost> Blogposts { get; set; }//after running migration this will create tables in db of name blogposts, we use Dbset for that
        public DbSet<Tag> Tags { get; set; }
        public DbSet<BlogPostLike> BlogPostLike { get; set; }
    }
}
