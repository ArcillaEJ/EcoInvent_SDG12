using System;

namespace EcoInvent.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string PasswordSalt { get; set; } = string.Empty;
        public string Role { get; set; } = "VIEWER";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}