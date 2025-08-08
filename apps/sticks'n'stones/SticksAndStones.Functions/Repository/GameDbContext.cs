using Microsoft.EntityFrameworkCore;
using SticksAndStones.Models;
using System;
using System.Linq;

namespace SticksAndStones.Repository;

public class GameDbContext(DbContextOptions<GameDbContext> options) : DbContext(options)
{
   public DbSet<Player> Players { get; set; }

   public DbSet<Match> Matches { get; set; }

   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
      modelBuilder.Entity<Player>()
         .HasKey(player => player.Id);

      modelBuilder.Entity<Match>()
         .HasKey(match => match.Id);

      modelBuilder.Entity<Match>()
         .Property(match => match.Sticks)
         .HasConversion(
            toDb => string.Join(",", toDb),
            fromDb => fromDb.Split(',', StringSplitOptions.None).Select(int.Parse).ToList());

      modelBuilder.Entity<Match>()
         .Property(match => match.Stones)
         .HasConversion(
            toDb => string.Join(",", toDb),
            fromDb => fromDb.Split(',', StringSplitOptions.None).Select(int.Parse).ToList());

      base.OnModelCreating(modelBuilder);
   }
}