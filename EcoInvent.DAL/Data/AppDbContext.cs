using EcoInvent.Models;
using Microsoft.EntityFrameworkCore;

namespace EcoInvent.DAL.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Item> Items => Set<Item>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Category> Categories => Set<Category>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>(entity =>
            {
                entity.HasKey(x => x.ItemId);
                entity.Property(x => x.ItemName).IsRequired().HasMaxLength(150);
                entity.Property(x => x.CurrentStock).IsRequired();
                entity.Property(x => x.ReorderLevel).IsRequired();

                entity.HasOne(x => x.Category)
                    .WithMany(c => c.Items)
                    .HasForeignKey(x => x.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(x => x.CategoryId);
                entity.Property(x => x.CategoryName).IsRequired().HasMaxLength(100);
                entity.HasIndex(x => x.CategoryName).IsUnique();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(x => x.UserId);
                entity.Property(x => x.Username).IsRequired().HasMaxLength(50);
                entity.Property(x => x.PasswordHash).IsRequired();
                entity.Property(x => x.PasswordSalt).IsRequired();
                entity.Property(x => x.Role).IsRequired().HasMaxLength(20);
                entity.HasIndex(x => x.Username).IsUnique();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}