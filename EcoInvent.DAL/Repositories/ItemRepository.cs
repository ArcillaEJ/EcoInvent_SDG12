using EcoInvent.DAL.Data;
using EcoInvent.Models;
using Microsoft.EntityFrameworkCore;

namespace EcoInvent.DAL.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly AppDbContext _context;

        public ItemRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Item>> GetAllAsync()
        {
            return await _context.Items
                .Include(x => x.Category)
                .OrderBy(x => x.ItemName)
                .ToListAsync();
        }

        public async Task<Item?> GetByIdAsync(int id)
        {
            return await _context.Items
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.ItemId == id);
        }

        public async Task<Item?> GetByNameAsync(string itemName)
        {
            return await _context.Items
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.ItemName == itemName);
        }

        public async Task AddAsync(Item item)
        {
            await _context.Items.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Item item)
        {
            _context.Items.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Item item)
        {
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}