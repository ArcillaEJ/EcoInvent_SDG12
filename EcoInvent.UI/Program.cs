using System;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;
using EcoInvent.BLL.Services;
using EcoInvent.DAL.Data;
using EcoInvent.DAL.Repositories;
using EcoInvent.Models;
using Microsoft.EntityFrameworkCore;

namespace EcoInvent.UI
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            Application.ThreadException += (s, e) => Logger.Error("UI Thread Exception", e.Exception);
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                if (e.ExceptionObject is Exception ex) Logger.Error("Unhandled Exception", ex);
            };

            Logger.SetLogFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ecoinvent.log"));

            try
            {
                string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "inventory.db");

                var options = new DbContextOptionsBuilder<AppDbContext>()
                    .UseSqlite($"Data Source={dbPath}")
                    .Options;

                using var db = new AppDbContext(options);
                db.Database.EnsureCreated();

                var itemRepo = new ItemRepository(db);
                var userRepo = new UserRepository(db);
                var categoryRepo = new CategoryRepository(db);

                SeedDefaultCategories(categoryRepo);
                SeedOrResetUser(userRepo, "admin", "admin123", "ADMIN");
                SeedOrResetUser(userRepo, "viewer", "viewer123", "VIEWER");

                var inventoryService = new InventoryService(itemRepo, categoryRepo);
                var authService = new AuthService(userRepo);

                // LOOP FOR LOGOUT FUNCTIONALITY
                bool sessionActive = true;
                while (sessionActive)
                {
                    using var login = new LoginForm(authService);
                    if (login.ShowDialog() != DialogResult.OK) 
                    {
                        sessionActive = false;
                        break;
                    }

                    using var mainForm = new InventoryForm(inventoryService, login.LoggedInRole);
                    var result = mainForm.ShowDialog();

                    if (result != DialogResult.Retry) // We use Retry for Logout
                    {
                        sessionActive = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Startup failed.", ex);
                var inner = ex.InnerException?.Message ?? "(no inner exception)";
                MessageBox.Show(
                    $"Startup failed.\n\nTop error:\n{ex.Message}\n\nInner error:\n{inner}",
                    "EcoInvent Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private static void SeedDefaultCategories(ICategoryRepository repo)
        {
            repo.GetOrCreateAsync("Paper").GetAwaiter().GetResult();
            repo.GetOrCreateAsync("Plastic").GetAwaiter().GetResult();
            repo.GetOrCreateAsync("Electronics").GetAwaiter().GetResult();
            repo.GetOrCreateAsync("Cleaning Supplies").GetAwaiter().GetResult();
        }

        private static void SeedOrResetUser(IUserRepository repo, string username, string password, string role)
        {
            var existing = repo.GetByUsernameAsync(username).GetAwaiter().GetResult();

            string salt = PasswordHasher.GenerateSalt();
            string hash = PasswordHasher.HashPassword(password, salt);

            if (existing == null)
            {
                var user = new User
                {
                    Username = username,
                    PasswordHash = hash,
                    PasswordSalt = salt,
                    Role = role,
                    CreatedAt = DateTime.UtcNow
                };
                repo.AddAsync(user).GetAwaiter().GetResult();
            }
            else
            {
                existing.PasswordHash = hash;
                existing.PasswordSalt = salt;
                existing.Role = role;
                existing.CreatedAt = existing.CreatedAt == default ? DateTime.UtcNow : existing.CreatedAt;

                repo.UpdateAsync(existing).GetAwaiter().GetResult();
            }
        }
    }
}