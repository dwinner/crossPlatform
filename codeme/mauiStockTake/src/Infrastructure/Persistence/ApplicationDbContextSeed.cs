using MauiStockTake.Domain.Entities;
using MauiStockTake.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace MauiStockTake.Infrastructure.Persistence;

public static class ApplicationDbContextSeed
{
    public static async Task SeedDefaultUserAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        var administratorRole = new IdentityRole("Administrator");

        if (roleManager.Roles.All(r => r.Name != administratorRole.Name))
        {
            await roleManager.CreateAsync(administratorRole);
        }

        var administrator = new ApplicationUser { UserName = "administrator@localhost", Email = "administrator@localhost" };

        if (userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            await userManager.CreateAsync(administrator, "Administrator1!");
            await userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
        }
    }

    public static async Task SeedSampleDataAsync(ApplicationDbContext context)
    {
        // Seed, if necessary
        if (!context.Manufacturers.Any())
        {
            context.Manufacturers.Add(new Manufacturer
            {
                Name = "Bobbie's Surf Supplies",
                Products =
                {
                    new Product { Name = "SoyBoy Board Wax", BarCode = "ABC123" },
                    new Product { Name = "Leggy Leash", BarCode = "ABC456" },
                    new Product { Name = "Fin pack", BarCode = "ABC789" },
                }
            });

            context.Manufacturers.Add(new Manufacturer
            {
                Name = "Mad Lad Boards",
                Products =
                {
                    new Product { Name = "Mad Longboard", BarCode = "DEF123" },
                    new Product { Name = "Mad Shortboard", BarCode = "DEF456" },
                    new Product { Name = "Mad Custom", BarCode = "DEF456" },
                }
            });

            context.Manufacturers.Add(new Manufacturer
            {
                Name = "Natura Surf Fashion",
                Products =
                {
                    new Product { Name = "Men's rashie", BarCode = "GHI123" },
                    new Product { Name = "Women's rashie", BarCode = "GHI456" },
                    new Product { Name = "Unisex signlet", BarCode = "GHI456" },
                }
            });

            await context.SaveChangesAsync();
        }
    }
}
