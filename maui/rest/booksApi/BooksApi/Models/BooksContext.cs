using Microsoft.EntityFrameworkCore;

namespace BooksApi.Models;

public partial class BooksContext(DbContextOptions<BooksContext> options) : DbContext(options)
{
   /*private const string ConnString =
      @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Books;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False";*/

   public virtual DbSet<Book> Books { get; set; }

   public virtual DbSet<UserInfo> UserInfos { get; set; }

   /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
   {
      optionsBuilder.UseSqlServer(ConnString);
      base.OnConfiguring(optionsBuilder);
   }*/

   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
      modelBuilder.Entity<Book>(entity =>
      {
         entity.Property(e => e.Author)
            .IsRequired()
            .HasMaxLength(50);

         entity.Property(e => e.Isbn).HasColumnName("ISBN").HasMaxLength(50);

         entity.Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(50);
      });

      modelBuilder.Entity<UserInfo>().HasKey(e => e.UserId);

      OnModelCreatingPartial(modelBuilder);
   }

   partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}