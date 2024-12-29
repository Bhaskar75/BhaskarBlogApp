using BhaskarBlogApp.Data;
using BhaskarBlogApp.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace BhaskarBlogApp.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly BloggieDbContext _bloggieDbContext;
        public TagRepository(BloggieDbContext bloggieDbContext)
        {
            _bloggieDbContext = bloggieDbContext;
        }

        public async Task<Tag> AddAsync(Tag tag)
        {
            await _bloggieDbContext.Tags.AddAsync(tag);

            //The SaveChanges method commits the changes to the database:

            //_bloggieDbContext.SaveChanges();  //Without this line anything cant be saved into the database
            await _bloggieDbContext.SaveChangesAsync();
            return tag;
        }

        public async Task<Tag?> DeleteAsync(Guid id)
        {
            var eT = await _bloggieDbContext.Tags.FindAsync(id);
            if (eT != null)
            {
                _bloggieDbContext.Tags.Remove(eT);
                await _bloggieDbContext.SaveChangesAsync();
                return eT;
            }
            return null;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await _bloggieDbContext.Tags.ToListAsync();
        }

        public Task<Tag?> GetAsync(Guid id)
        {
            return _bloggieDbContext.Tags.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Tag?> UpdateAsync(Tag tag)
        {
            var existingTag = await _bloggieDbContext.Tags.FindAsync(tag.Id);
            if (existingTag != null)
            {
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;
                await _bloggieDbContext.SaveChangesAsync();
                return existingTag;
            }

            return null;
        }
    }
}
