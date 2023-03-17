using Microsoft.EntityFrameworkCore;
using WebBlog.Data;
using WebBlog.Data.Entity.Blog;

namespace WebBlog.Services.Blog
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllCategories();
        Task<Category> GetById(int categoryId);
    }
    public class CategoryService : ICategoryService
    {
        private AppDbContext DbContext { get; }

        public CategoryService(AppDbContext dbContext)
        { 
            DbContext = dbContext;
        }
        public async Task<List<Category>> GetAllCategories()
        {
            return await DbContext.Categories.ToListAsync();
        }

        public async Task<Category> GetById(int categoryId)
        {
            return await DbContext.Categories.SingleAsync(e => e.Id == categoryId);
        }
    }
}
