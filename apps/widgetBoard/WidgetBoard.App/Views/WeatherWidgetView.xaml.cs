using WidgetBoard.App.ViewModels;

namespace WidgetBoard.App.Views;

public partial class WeatherWidgetView : ContentView, IWidgetView
{
   public WeatherWidgetView()
   {
      InitializeComponent();
   }

   public IWidgetViewModel WidgetViewModel
   {
      get => (IWidgetViewModel)BindingContext;
      set => BindingContext = value;
   }
}