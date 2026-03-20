using EcoInvent.DAL.Data;
using EcoInvent.Models;
using Microsoft.EntityFrameworkCore;

namespace EcoInvent.DAL.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _context.Categories
                .OrderBy(x => x.CategoryName)
                .ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _context.Categories.FirstOrDefaultAsync(x => x.CategoryId == id);
        }

        public async Task<Category?> GetByNameAsync(string name)
        {
            return await _context.Categories.FirstOrDefaultAsync(x => x.CategoryName == name);
        }

        public async Task<Category> GetOrCreateAsync(string categoryName)
        {
            string clean = categoryName.Trim();

            var existing = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryName == clean);
            if (existing != null) return existing;

            var category = new Category
            {
                CategoryName = clean
            };

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return category;
        }
    }
}