using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StockTake.Infrastructure.Identity;
using StockTake.Infrastructure.Persistence;

namespace StockTake.WebServices;

public class Program
{
   public static async Task Main(string[] args)
   {
      var host = CreateHostBuilder(args).Build();

      using (var scope = host.Services.CreateScope())
      {
         var services = scope.ServiceProvider;

         try
         {
            var context = services.GetRequiredService<ApplicationDbContext>();
            if (context.Database.IsSqlServer())
            {
               await context.Database.MigrateAsync();
            }

            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            await ApplicationDbContextSeed.SeedDefaultUserAsync(userManager, roleManager);
            await ApplicationDbContextSeed.SeedSampleDataAsync(context);
         }
         catch (Exception ex)
         {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while migrating or seeding the database.");

            throw;
         }
      }

      await host.RunAsync();
   }

   public static IHostBuilder CreateHostBuilder(string[] args) =>
      Host.CreateDefaultBuilder(args)
         .ConfigureWebHostDefaults(webBuilder =>
            webBuilder.UseStartup<Startup>());
}