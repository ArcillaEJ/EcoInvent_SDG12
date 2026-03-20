namespace EcoInvent.Models
{
    public class InventoryItemView
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int CurrentStock { get; set; }
        public int ReorderLevel { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}