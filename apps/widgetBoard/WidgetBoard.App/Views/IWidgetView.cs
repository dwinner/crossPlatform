using WidgetBoard.App.ViewModels;

namespace WidgetBoard.App.Views;

public interface IWidgetView
{
   IWidgetViewModel WidgetViewModel { get; set; }

   int Position
   {
      get => WidgetViewModel.Position;
      set => WidgetViewModel.Position = value;
   }
}