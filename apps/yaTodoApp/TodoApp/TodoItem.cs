using SQLite;

namespace TodoApp;

public class TodoItem
{
   [PrimaryKey, AutoIncrement]
   public int Id { get; set; }

   public string Title { get; set; } = string.Empty;

   public DateTime Due { get; set; }

   public bool Done { get; set; } = false;
}