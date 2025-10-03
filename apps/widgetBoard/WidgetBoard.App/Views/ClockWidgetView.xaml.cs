using WidgetBoard.App.ViewModels;

namespace WidgetBoard.App.Views;

public partial class ClockWidgetView : IWidgetView
{
   public ClockWidgetView()
   {
      InitializeComponent();
   }

   public IWidgetViewModel WidgetViewModel
   {
      get => (IWidgetViewModel)BindingContext;
      set => BindingContext = value;
   }
}