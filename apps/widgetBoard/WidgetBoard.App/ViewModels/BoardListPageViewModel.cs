using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WidgetBoard.App.Data;
using WidgetBoard.App.Models;

namespace WidgetBoard.App.ViewModels;

public partial class BoardListPageViewModel(IBoardRepository boardRepository) : ViewModelBase
{
   [ObservableProperty] private Board? _currentBoard;

   public ObservableCollection<Board> Boards { get; } = [];

   partial void OnCurrentBoardChanged(Board? oldValue, Board? newValue)
   {
      if (newValue != oldValue && newValue is not null)
      {
         BoardSelected(newValue);
      }
   }

   private async void BoardSelected(Board board)
   {
      await Shell.Current.GoToAsync(
         RouteNames.FixedBoard,
         new Dictionary<string, object> { { "Board", board } }
      );
   }

   [RelayCommand]
   private async Task AddBoard()
   {
      TaskCompletionSource<Board?> boardCreated = new();
      await Shell.Current.GoToAsync(
         RouteNames.BoardDetails,
         new Dictionary<string, object> { { "Created", boardCreated } }
      );

      var newBoard = await boardCreated.Task;
      if (newBoard is not null)
      {
         Boards.Add(newBoard);
      }
   }

   public void LoadBoards()
   {
      Boards.Clear();
      var boards = boardRepository.ListBoards();
      foreach (var board in boards)
      {
         Boards.Add(board);
      }
   }
}