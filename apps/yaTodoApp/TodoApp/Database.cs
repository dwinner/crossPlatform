using SQLite;

namespace TodoApp;

public class Database
{
   private readonly SQLiteAsyncConnection _connection;

   public Database()
   {
      var dataDir = FileSystem.AppDataDirectory;
      var databasePath = Path.Combine(dataDir, "MauiTodo.db");
      var dbEncryptionKey = SecureStorage.GetAsync("dbKey").Result;
      if (string.IsNullOrEmpty(dbEncryptionKey))
      {
         var guid = Guid.Empty;
         dbEncryptionKey = guid.ToString();
         SecureStorage.SetAsync("dbKey", dbEncryptionKey);
      }

      var dbOptions = new SQLiteConnectionString(databasePath, true, dbEncryptionKey);
      _connection = new SQLiteAsyncConnection(dbOptions);

      _ = Initialise();
   }

   private async Task Initialise()
   {
      await _connection.CreateTableAsync<TodoItem>();
   }

   public async Task<List<TodoItem>> GetTodos() => await _connection.Table<TodoItem>().ToListAsync();

   public async Task<TodoItem> GetTodo(int id)
   {
      var query = _connection.Table<TodoItem>().Where(t => t.Id == id);
      return await query.FirstOrDefaultAsync();
   }

   public async Task<int> AddTodo(TodoItem item) => await _connection.InsertAsync(item);

   public async Task<int> DeleteTodo(TodoItem item) => await _connection.DeleteAsync(item);

   public async Task<int> UpdateTodo(TodoItem item) => await _connection.UpdateAsync(item);
}