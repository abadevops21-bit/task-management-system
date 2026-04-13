using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(ApplicationDbContext context)
        {
            // Apply pending migrations
            if (context.Database.GetPendingMigrations().Any())
            {
                await context.Database.MigrateAsync();
            }

            // Seed SuperUser (only if not exists)
            if (!context.Users.Any(u => u.Role == "SuperUser"))
            {
                var superUser = new User
                {
                    Id = Guid.NewGuid(),
                    Name = "Super Admin",
                    Email = "superadmin@test.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"),
                    Role = "SuperUser",
                    CreatedAt = DateTime.UtcNow
                };

                context.Users.Add(superUser);
                await context.SaveChangesAsync();
            }

            // Seed Admin
            if (!context.Users.Any(u => u.Role == "Admin"))
            {
                var admin = new User
                {
                    Id = Guid.NewGuid(),
                    Name = "Admin User",
                    Email = "admin@test.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"),
                    Role = "Admin",
                    CreatedAt = DateTime.UtcNow
                };

                context.Users.Add(admin);
                await context.SaveChangesAsync();
            }
        }
    }
}