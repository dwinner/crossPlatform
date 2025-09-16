using SQLite;

namespace WidgetBoard.App.Models;

public class BoardWidget
{
   [PrimaryKey, AutoIncrement]
   public int Id { get; set; }

   public int BoardId { get; set; }

   public int Position { get; set; }

   public string WidgetType { get; set; } = string.Empty;
}