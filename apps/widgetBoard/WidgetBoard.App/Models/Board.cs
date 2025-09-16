using SQLite;

namespace WidgetBoard.App.Models;

public class Board
{
   [PrimaryKey, AutoIncrement]
   public int Id { get; set; }

   public string Name { get; set; } = string.Empty;

   public int NumberOfColumns { get; set; }

   public int NumberOfRows { get; set; }

   [Ignore]
   public IReadOnlyList<BoardWidget> BoardWidgets { get; set; } = [];
}