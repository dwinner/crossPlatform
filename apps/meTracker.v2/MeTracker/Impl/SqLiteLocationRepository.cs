using System.Diagnostics;
using MeTracker.Models;
using MeTracker.Services;
using SQLite;

namespace MeTracker.Impl;

internal class SqLiteLocationRepository : ILocationRepository
{
   private const string DbName = "Locations.db";
   private SQLiteAsyncConnection? _connection;

   public async Task<List<Location>> GetAllAsync()
   {
      await CreateConnectionAsync().ConfigureAwait(false);
      var locations = await _connection!.Table<Location>().ToListAsync()
         .ConfigureAwait(false);

      return locations;
   }

   public async Task SaveAsync(LocationEntry location)
   {
      await CreateConnectionAsync().ConfigureAwait(false);
      await _connection!.InsertAsync(location).ConfigureAwait(false);
   }

   private async Task CreateConnectionAsync()
   {
      if (_connection != null)
      {
         return;
      }

      var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), DbName);

      _connection = new SQLiteAsyncConnection(dbPath);
      await _connection.CreateTableAsync<Location>().ConfigureAwait(false);
      Debug.WriteLine("Connection ia active");
   }
}