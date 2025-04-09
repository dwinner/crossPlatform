using DoToo.Models;

namespace DoToo.Repositories;

public interface ITodoItemRepository
{
   event EventHandler<TodoItem> OnItemAdded;

   event EventHandler<TodoItem> OnItemUpdated;

   Task<List<TodoItem>> GetItemsAsync();

   Task AddItemAsync(TodoItem anItem);

   Task UpdateItemAsync(TodoItem anItem);

   Task AddOrUpdateAsync(TodoItem anItem);
}