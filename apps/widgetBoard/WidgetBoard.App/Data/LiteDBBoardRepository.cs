using LiteDB;
using WidgetBoard.App.Models;

namespace WidgetBoard.App.Data;

internal sealed class LiteDbBoardRepository : IBoardRepository
{
   private const string DbName = "widgetboard_litedb.db";
   private const string BoardTableName = "Boards";
   private const string BoardWidgetTableName = "BoardWidgets";
   private readonly ILiteCollection<Board> _boardCollection;
   private readonly ILiteCollection<BoardWidget> _boardWidgetCollection;

   public LiteDbBoardRepository(IFileSystem fileSystem)
   {
      var dbPath = Path.Combine(fileSystem.AppDataDirectory, DbName);
      var database = new LiteDatabase(dbPath);

      _boardCollection = database.GetCollection<Board>(BoardTableName);
      _boardWidgetCollection = database.GetCollection<BoardWidget>(BoardWidgetTableName);

      _boardCollection.EnsureIndex(board => board.Id, true);
      _boardCollection.EnsureIndex(board => board.Name);
   }

   public void CreateBoard(Board board)
   {
      _boardCollection.Insert(board);
   }

   public void CreateBoardWidget(BoardWidget boardWidget)
   {
      _boardWidgetCollection.Insert(boardWidget);
   }

   public void DeleteBoard(Board board)
   {
      _boardCollection.Delete(board.Id);
   }

   public IReadOnlyList<Board> ListBoards()
   {
      return _boardCollection
         .Query()
         .OrderBy(board => board.Name)
         .ToList();
   }

   public Board? LoadBoard(int boardId)
   {
      var board = _boardCollection.FindById(boardId);
      if (board is null)
      {
         return null;
      }

      var boardWidgets = _boardWidgetCollection
         .Find(widget => widget.BoardId == boardId)
         .ToList();
      board.BoardWidgets = boardWidgets;

      return board;
   }

   public void UpdateBoard(Board board)
   {
      _boardCollection.Update(board);
   }
}