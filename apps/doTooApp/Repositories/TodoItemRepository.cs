using System.Diagnostics;
using DoToo.Models;
using SQLite;
using static System.Environment;

namespace DoToo.Repositories;

public class TodoItemRepository : ITodoItemRepository
{
   private const string TodoDbName = "TodoItems.db";
   private SQLiteAsyncConnection? _connection;
   public event EventHandler<TodoItem>? OnItemAdded;
   public event EventHandler<TodoItem>? OnItemUpdated;

   public async Task<List<TodoItem>> GetItemsAsync()
   {
      await CreateConnectionAsync().ConfigureAwait(false);
      Debug.Assert(_connection != null, $"{nameof(_connection)} != null");
      var items = await _connection.Table<TodoItem>().ToListAsync()
         .ConfigureAwait(true);
      return items;
   }

   public async Task AddItemAsync(TodoItem anItem)
   {
      await CreateConnectionAsync().ConfigureAwait(false);
      Debug.Assert(_connection != null, nameof(_connection) + " != null");
      await _connection.InsertAsync(anItem)
         .ConfigureAwait(true);
      OnItemAdded?.Invoke(this, anItem);
   }

   public async Task UpdateItemAsync(TodoItem anItem)
   {
      await CreateConnectionAsync().ConfigureAwait(false);
      Debug.Assert(_connection != null, nameof(_connection) + " != null");
      await _connection.UpdateAsync(anItem)
         .ConfigureAwait(true);
      OnItemUpdated?.Invoke(this, anItem);
   }

   public async Task AddOrUpdateAsync(TodoItem anItem)
   {
      Func<TodoItem, Task> addOrUpdate = anItem.Id == 0
         ? AddItemAsync
         : UpdateItemAsync;
      await addOrUpdate(anItem)
         .ConfigureAwait(false);
      Debug.WriteLine($" Item '{anItem}' add/updated");
   }

   private async Task CreateConnectionAsync()
   {
      if (_connection != null)
      {
         return;
      }

      var documentPath = GetFolderPath(SpecialFolder.MyDocuments);
      if (!Directory.Exists(documentPath))
      {
         Directory.CreateDirectory(documentPath);
      }

      var databasePath = Path.Combine(documentPath, TodoDbName);
      _connection = new SQLiteAsyncConnection(databasePath);
      await _connection.CreateTableAsync<TodoItem>()
         .ConfigureAwait(true);
      var itemsCount = await _connection.Table<TodoItem>().CountAsync()
         .ConfigureAwait(true);
      if (itemsCount == 0)
      {
         await _connection.InsertAsync(new TodoItem
         {
            Title = "Welcome to DoToo",
            Due = DateTime.Now
         }).ConfigureAwait(true);
      }
   }
}