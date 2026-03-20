using System;

namespace EcoInvent.Models
{
    public class Item
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;

        public int CurrentStock { get; set; }
        public int ReorderLevel { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}