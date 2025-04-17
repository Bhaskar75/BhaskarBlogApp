using System.Diagnostics.Metrics;
using BhaskarBlogApp.Data;
using BhaskarBlogApp.Models.Domain;
using BhaskarBlogApp.Models.ViewModels;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;

namespace BhaskarBlogApp.Repositories
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly BloggieDbContext bloggieDbContext;
        public BlogPostRepository(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }
        public async Task<BlogPost> AddAsync(BlogPost blogPost)
        {
            await bloggieDbContext.AddAsync(blogPost);
            await bloggieDbContext.SaveChangesAsync();
            return blogPost;
        }

        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
            var existingBlog = await bloggieDbContext.Blogposts.FindAsync(id);
            if (existingBlog != null)
            {
                bloggieDbContext.Blogposts.Remove(existingBlog);
                await bloggieDbContext.SaveChangesAsync();
                return existingBlog;
            }
            return null;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await bloggieDbContext.Blogposts.Include(x => x.Tags).ToListAsync();
        }

        public async Task<BlogPost?> GetAsync(Guid id)
        {
            return await bloggieDbContext.Blogposts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BlogPost?> GetByUrlHandleAsync(string urlHandle)
        {
           return await bloggieDbContext.Blogposts.Include(x=>x.Tags).
                FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            var existingBlog = await bloggieDbContext.Blogposts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == blogPost.Id);

            if (existingBlog != null)
            {
                existingBlog.Id = blogPost.Id;
                existingBlog.Heading = blogPost.Heading;
                existingBlog.pageTitle = blogPost.pageTitle;
                existingBlog.Content = blogPost.Content;
                existingBlog.Author = blogPost.Author;
                existingBlog.ShortDescription = blogPost.ShortDescription;
                existingBlog.FeaturedImageUrl = blogPost.FeaturedImageUrl;
                existingBlog.PublishedDate = blogPost.PublishedDate;
                existingBlog.UrlHandle = blogPost.UrlHandle;
                existingBlog.Visible = blogPost.Visible;

                await bloggieDbContext.SaveChangesAsync();
                return existingBlog;
            }

            return null;
        }
    }
}
