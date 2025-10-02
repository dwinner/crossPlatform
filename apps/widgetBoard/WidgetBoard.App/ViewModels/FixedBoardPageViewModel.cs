using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WidgetBoard.App.Data;
using WidgetBoard.App.Models;

namespace WidgetBoard.App.ViewModels;

public partial class FixedBoardPageViewModel(
   WidgetTemplateSelector widgetTemplateSelector,
   WidgetFactory widgetFactory,
   IBoardRepository boardRepository,
   IPreferences preferences)
   : ViewModelBase, IQueryAttributable
{
   private int _addingPosition;
   private Board? _board;

   [ObservableProperty] private string _boardName = string.Empty;
   [ObservableProperty] private bool _isAddingWidget;
   [ObservableProperty] private int _numberOfColumns;
   [ObservableProperty] private int _numberOfRows;
   [ObservableProperty] private string? _selectedWidget;

   public IList<string> AvailableWidgets => widgetFactory.AvailableWidgets;

   public ObservableCollection<IWidgetViewModel> Widgets { get; } = [];

   public WidgetTemplateSelector WidgetTemplateSelector { get; } = widgetTemplateSelector;

   public void ApplyQueryAttributes(IDictionary<string, object> query)
   {
      var boardParameter = (Board)query["Board"];
      _board = boardRepository.LoadBoard(boardParameter.Id);

      if (_board is not null)
      {
         preferences.Set("LastUsedBoardId", _board.Id);
         BoardName = _board.Name;
         NumberOfColumns = _board.NumberOfColumns;
         NumberOfRows = _board.NumberOfRows;

         foreach (var boardWidget in _board.BoardWidgets)
         {
            var widgetViewModel = widgetFactory.CreateWidgetViewModel(boardWidget.WidgetType);
            if (widgetViewModel is null)
            {
               continue;
            }

            widgetViewModel.Position = boardWidget.Position;
            Widgets.Add(widgetViewModel);
         }
      }
   }

   [RelayCommand]
   private void AddNewWidget(int index)
   {
      IsAddingWidget = true;
      _addingPosition = index;
   }

   [RelayCommand]
   private void AddWidget()
   {
      if (SelectedWidget is null)
      {
         return;
      }

      var widgetViewModel = widgetFactory.CreateWidgetViewModel(SelectedWidget);
      if (widgetViewModel is not null)
      {
         widgetViewModel.Position = _addingPosition;
         Widgets.Add(widgetViewModel);
         SaveWidget(widgetViewModel);
      }

      IsAddingWidget = false;
   }

   private void SaveWidget(IWidgetViewModel widgetViewModel)
   {
      if (_board is null)
      {
         return;
      }

      var boardWidget = new BoardWidget
      {
         BoardId = _board.Id,
         Position = widgetViewModel.Position,
         WidgetType = widgetViewModel.Type
      };
      boardRepository.CreateBoardWidget(boardWidget);
   }
}