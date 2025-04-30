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

        public async Task<int> CountAsync()
        {
            return await _bloggieDbContext.Tags.CountAsync();
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

        public async Task<IEnumerable<Tag>> GetAllAsync(string? searchQuery, string? sortBy, string? sortDirection, int pageNumber = 1,
                                           int pageSize = 100)
        {
            //return await _bloggieDbContext.Tags.ToListAsync();
            var query = _bloggieDbContext.Tags.AsQueryable();
            //Filtering
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                query = query.Where(x => x.Name.Contains(searchQuery) ||
                                    x.DisplayName.Contains(searchQuery));
            }

            //Sorting
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                var isDesc = string.Equals(sortDirection, "Desc", StringComparison.OrdinalIgnoreCase);
                if (string.Equals(sortBy, "Name", StringComparison.OrdinalIgnoreCase))
                {
                    query = isDesc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name);
                }
                if (string.Equals(sortBy, "DisplayName", StringComparison.OrdinalIgnoreCase))
                {
                    query = isDesc ? query.OrderByDescending(x => x.DisplayName) : query.OrderBy(x => x.DisplayName);
                }
            }

            //Pagination
            //Skip 0 take 5-> Page 1 of 5 results
            var skipResults = (pageNumber - 1) * pageSize;
            query = query.Skip(skipResults).Take(pageSize);

            return await query.ToListAsync();
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
