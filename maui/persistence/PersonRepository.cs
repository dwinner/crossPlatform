using System.Diagnostics;
using People.Models;
using SQLite;

namespace People;

public class PersonRepository(string dbPath)
{
   private SQLiteAsyncConnection _conn;

   public string StatusMessage { get; private set; }

   private async Task InitAsync()
   {
      if (_conn != null)
      {
         return;
      }

      _conn = new SQLiteAsyncConnection(dbPath);
      await _conn.CreateTableAsync<Person>().ConfigureAwait(false);
      Debug.WriteLine($"{nameof(PersonRepository)}.{nameof(InitAsync)}");
   }

   public async Task AddAsync(string aPersonName)
   {
      try
      {
         await InitAsync().ConfigureAwait(false);

         // basic validation to ensure a name was entered
         if (string.IsNullOrEmpty(aPersonName))
         {
            throw new Exception("Valid name required");
         }

         var result = await _conn.InsertAsync(new Person { Name = aPersonName }).ConfigureAwait(false);
         StatusMessage = $"{result} record(s) added (Name: {aPersonName})";
      }
      catch (Exception ex)
      {
         StatusMessage = $"Failed to add {aPersonName}. Error: {ex.Message}";
      }
   }

   public async Task<List<Person>> GetAllAsync()
   {
      try
      {
         await InitAsync().ConfigureAwait(false);
         var persons = await _conn.Table<Person>().ToListAsync().ConfigureAwait(false);

         return persons;
      }
      catch (Exception ex)
      {
         StatusMessage = $"Failed to retrieve data. {ex.Message}";
      }

      return new List<Person>();
   }
}