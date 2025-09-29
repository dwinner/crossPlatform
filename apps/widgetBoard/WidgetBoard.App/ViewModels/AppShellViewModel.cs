using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using WidgetBoard.App.Data;
using WidgetBoard.App.Models;

namespace WidgetBoard.App.ViewModels;

public partial class AppShellViewModel : ViewModelBase
{
   private readonly IBoardRepository _boardRepository;
   private readonly IDispatcher _dispatcher;
   private readonly IPreferences _preferences;

   [ObservableProperty] private Board? _currentBoard;

   public AppShellViewModel(IBoardRepository boardRepository, IPreferences preferences, IDispatcher dispatcher)
   {
      _boardRepository = boardRepository;
      _preferences = preferences;
      _dispatcher = dispatcher;
   }

   public ObservableCollection<Board> Boards { get; } = [];

   partial void OnCurrentBoardChanged(Board? oldValue, Board? newValue)
   {
      if (newValue != oldValue && newValue is not null)
      {
         BoardSelected(newValue);
      }
   }

   private async void BoardSelected(Board aBoard)
   {
      await Shell.Current.GoToAsync(RouteNames.FixedBoard,
         new Dictionary<string, object>
         {
            { "Board", aBoard }
         });
   }

   public void LoadBoards()
   {
      Boards.Clear();

      var boards = _boardRepository.ListBoards();
      var lastUsedBoardId = _preferences.Get("LastUsedBoardId", -1);
      Board? lastUsedBoard = null;
      foreach (var board in boards)
      {
         Boards.Add(board);
         if (lastUsedBoardId == board.Id)
         {
            lastUsedBoard = board;
         }
      }

      if (lastUsedBoard is not null)
      {
         _dispatcher.Dispatch(() => { BoardSelected(lastUsedBoard); });
      }
   }
}