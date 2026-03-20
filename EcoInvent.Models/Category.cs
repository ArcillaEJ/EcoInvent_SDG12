using System.Collections.Generic;

namespace EcoInvent.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;

        public ICollection<Item> Items { get; set; } = new List<Item>();
    }
}