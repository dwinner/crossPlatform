using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WidgetBoard.App.Data;
using WidgetBoard.App.Models;

namespace WidgetBoard.App.ViewModels;

public partial class BoardDetailsPageViewModel(
   ISemanticScreenReader semanticScreenReader,
   IBoardRepository boardRepository) : ViewModelBase, IQueryAttributable
{
   [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
   private string _boardName = string.Empty;

   [ObservableProperty] private bool _isFixed = true;

   [ObservableProperty] private int _numberOfColumns = 3;

   [ObservableProperty] private int _numberOfRows = 2;

   public TaskCompletionSource<Board?>? BoardCreatedCompletionSource { get; set; }

   private bool CanSave => !string.IsNullOrWhiteSpace(BoardName);

   public void ApplyQueryAttributes(IDictionary<string, object> query)
   {
      BoardCreatedCompletionSource = query["Created"] as TaskCompletionSource<Board?>;
   }

   [RelayCommand]
   private async Task Cancel()
   {
      await Shell.Current.GoToAsync("..");
      BoardCreatedCompletionSource?.SetResult(null);
   }

   [RelayCommand(CanExecute = nameof(CanSave))]
   private void Save()
   {
      var board = new Board
      {
         Name = BoardName,
         NumberOfColumns = NumberOfColumns,
         NumberOfRows = NumberOfRows
      };

      boardRepository.CreateBoard(board);
      semanticScreenReader.Announce($"A new board with the name {BoardName} was created successfully.");
      Shell.Current.GoToAsync("..");
      BoardCreatedCompletionSource?.SetResult(board);
   }
}