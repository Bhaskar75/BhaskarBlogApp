using Microsoft.AspNetCore.Identity;

namespace BhaskarBlogApp.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<IdentityUser>> GetAll();
    }
}
