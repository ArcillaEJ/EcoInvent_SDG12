namespace EcoInvent.Models;

public class Transaction
{
    public int TransactionId { get; set; }

    public int ItemId { get; set; }
    public Item? Item { get; set; }

    public string Type { get; set; } = "IN"; // IN / OUT
    public int Quantity { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string Notes { get; set; } = "";
}