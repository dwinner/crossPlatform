using c4_LocalDatabaseConnection.DataAccess;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace c4_LocalDatabaseConnection.ViewModels;

public class CrmContext : DbContext
{
   private const string DbName = "localdatabase.db";

   public CrmContext()
   {
      Batteries_V2.Init();
      Database.EnsureCreated();
   }

   public DbSet<Customer> Customers { get; set; }

   protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
   {
      var dbPath = Path.Combine(FileSystem.AppDataDirectory, DbName);
      optionsBuilder.UseSqlite($"Filename={dbPath}");
      base.OnConfiguring(optionsBuilder);
   }

   protected override void OnModelCreating(ModelBuilder builder)
   {
      builder.Entity<Customer>()
         .HasIndex(c => c.Email)
         .IsUnique();
   }
}