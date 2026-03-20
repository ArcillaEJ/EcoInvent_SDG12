using System.Text.Json;
using EcoInvent.DAL.Repositories;
using EcoInvent.Models;

namespace EcoInvent.BLL.Services
{
    public class InventoryService
    {
        private readonly IItemRepository _itemRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly Logger _logger;

        public InventoryService(IItemRepository itemRepository, ICategoryRepository categoryRepository)
        {
            _itemRepository = itemRepository;
            _categoryRepository = categoryRepository;
            _logger = new Logger();
        }

        public async Task<List<InventoryItemView>> GetItemsAsync()
        {
            try
            {
                var items = await _itemRepository.GetAllAsync();

                return items.Select(i => new InventoryItemView
                {
                    ItemId = i.ItemId,
                    ItemName = i.ItemName,
                    Category = i.Category?.CategoryName ?? "Unknown",
                    CurrentStock = i.CurrentStock,
                    ReorderLevel = i.ReorderLevel,
                    Status = GetStockStatus(i.CurrentStock, i.ReorderLevel)
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "InventoryService.GetItemsAsync");
                throw;
            }
        }

        public async Task AddItemAsync(string itemName, string categoryName, int stock, int reorderLevel)
        {
            try
            {
                Validate(itemName, categoryName, stock, reorderLevel);

                var category = await _categoryRepository.GetOrCreateAsync(categoryName);

                var item = new Item
                {
                    ItemName = itemName.Trim(),
                    CategoryId = category.CategoryId,
                    CurrentStock = stock,
                    ReorderLevel = reorderLevel,
                    CreatedAt = DateTime.UtcNow
                };

                await _itemRepository.AddAsync(item);
                _logger.LogInfo($"Item added: {itemName}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "InventoryService.AddItemAsync");
                throw;
            }
        }

        public async Task UpdateItemAsync(int id, string itemName, string categoryName, int stock, int reorderLevel)
        {
            try
            {
                Validate(itemName, categoryName, stock, reorderLevel);

                var item = await _itemRepository.GetByIdAsync(id);
                if (item == null)
                    throw new Exception("Item not found.");

                var category = await _categoryRepository.GetOrCreateAsync(categoryName);

                item.ItemName = itemName.Trim();
                item.CategoryId = category.CategoryId;
                item.CurrentStock = stock;
                item.ReorderLevel = reorderLevel;

                await _itemRepository.UpdateAsync(item);
                _logger.LogInfo($"Item updated: {itemName}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "InventoryService.UpdateItemAsync");
                throw;
            }
        }

        public async Task DeleteItemAsync(int id)
        {
            try
            {
                var item = await _itemRepository.GetByIdAsync(id);
                if (item == null)
                    throw new Exception("Item not found.");

                await _itemRepository.DeleteAsync(item);
                _logger.LogInfo($"Item deleted: {item.ItemName}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "InventoryService.DeleteItemAsync");
                throw;
            }
        }

        public async Task BackupToJsonAsync(string path)
        {
            try
            {
                var items = await GetItemsAsync();

                string json = JsonSerializer.Serialize(items, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                await File.WriteAllTextAsync(path, json);

                _logger.LogInfo($"Backup created: {path}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "InventoryService.BackupToJsonAsync");
                throw;
            }
        }

        public async Task RestoreFromJsonAsync(string path)
        {
            try
            {
                if (!File.Exists(path))
                    throw new Exception("Backup file not found.");

                string json = await File.ReadAllTextAsync(path);
                var items = JsonSerializer.Deserialize<List<InventoryItemView>>(json) ?? new List<InventoryItemView>();

                foreach (var row in items)
                {
                    var existing = await _itemRepository.GetByIdAsync(row.ItemId);

                    if (existing == null)
                    {
                        await AddItemAsync(row.ItemName, row.Category, row.CurrentStock, row.ReorderLevel);
                    }
                    else
                    {
                        await UpdateItemAsync(existing.ItemId, row.ItemName, row.Category, row.CurrentStock, row.ReorderLevel);
                    }
                }

                _logger.LogInfo($"Restore completed: {path}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "InventoryService.RestoreFromJsonAsync");
                throw;
            }
        }

        public string GetStockStatus(int stock, int reorderLevel)
        {
            if (stock == 0) return "OUT OF STOCK";
            if (stock <= reorderLevel) return "REORDER";
            return "OK";
        }

        private void Validate(string itemName, string categoryName, int stock, int reorderLevel)
        {
            if (string.IsNullOrWhiteSpace(itemName))
                throw new Exception("Item name is required.");

            if (string.IsNullOrWhiteSpace(categoryName))
                throw new Exception("Category is required.");

            if (stock < 0)
                throw new Exception("Stock cannot be negative.");

            if (reorderLevel < 0)
                throw new Exception("Reorder level cannot be negative.");
        }
    }
}