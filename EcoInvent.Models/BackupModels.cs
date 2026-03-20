namespace EcoInvent.Models;

public class ItemsBackup
{
    public DateTime ExportedAt { get; set; } = DateTime.UtcNow;
    public List<Item> Items { get; set; } = new();
}