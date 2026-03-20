using EcoInvent.Models;

namespace EcoInvent.DAL.Repositories
{
    public interface IItemRepository
    {
        Task<List<Item>> GetAllAsync();
        Task<Item?> GetByIdAsync(int id);
        Task<Item?> GetByNameAsync(string itemName);
        Task AddAsync(Item item);
        Task UpdateAsync(Item item);
        Task DeleteAsync(Item item);
    }
}