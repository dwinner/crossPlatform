using System.Collections.Generic;
using System.IO;
using Microsoft.Maui.Storage;
using SQLite;
using WidgetBoard.App.Models;

namespace WidgetBoard.App.Data;

internal sealed class SqliteBoardRepository : IBoardRepository
{
   private const string DbName = "widgetboard_sqlite.db";
   private readonly SQLiteConnection _connection;

   public SqliteBoardRepository(IFileSystem fileSystem)
   {
      var dbPath = Path.Combine(fileSystem.AppDataDirectory, DbName);
      _connection = new SQLiteConnection(dbPath);
      _connection.CreateTable<Board>();
      _connection.CreateTable<BoardWidget>();
   }

   public void CreateBoard(Board board)
   {
      _connection.Insert(board);
   }

   public void CreateBoardWidget(BoardWidget boardWidget)
   {
      _connection.Insert(boardWidget);
   }

   public void DeleteBoard(Board board)
   {
      _connection.Delete(board);
   }

   public IReadOnlyList<Board> ListBoards()
   {
      return _connection.Table<Board>()
         .OrderBy(board => board.Name)
         .ToList();
   }

   public Board? LoadBoard(int boardId)
   {
      var board = _connection.Find<Board>(boardId);
      if (board is null)
      {
         return null;
      }

      var widgets = _connection.Table<BoardWidget>()
         .Where(widget => widget.BoardId == boardId)
         .ToList();
      board.BoardWidgets = widgets;

      return board;
   }

   public void UpdateBoard(Board board)
   {
      _connection.Update(board);
   }
}