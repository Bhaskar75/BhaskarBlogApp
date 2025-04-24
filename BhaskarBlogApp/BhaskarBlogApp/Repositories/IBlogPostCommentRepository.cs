using BhaskarBlogApp.Models.Domain;

namespace BhaskarBlogApp.Repositories
{
    public interface IBlogPostCommentRepository
    {
        Task<BlogPostComment> AddAsync(BlogPostComment blogPostComment);
    }
}
