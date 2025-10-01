using WidgetBoard.App.ViewModels;
using WidgetBoard.App.Views;

namespace WidgetBoard.App;

public class WidgetFactory(IServiceProvider serviceProvider)
{
   private static readonly Dictionary<Type, Type> WidgetRegistrations = new();

   private static readonly Dictionary<string, Type> WidgetNameRegistrations = new();

   public IList<string> AvailableWidgets => WidgetNameRegistrations.Keys.ToList();

   public static void RegisterWidget<TWidgetView, TWidgetViewModel>(string displayName)
      where TWidgetView : IWidgetView
      where TWidgetViewModel : IWidgetViewModel
   {
      WidgetRegistrations.Add(typeof(TWidgetViewModel), typeof(TWidgetView));
      WidgetNameRegistrations.Add(displayName, typeof(TWidgetViewModel));
   }

   public IWidgetView? CreateWidget(IWidgetViewModel widgetViewModel)
   {
      if (WidgetRegistrations.TryGetValue(widgetViewModel.GetType(), out var widgetViewType))
      {
         var widgetView = (IWidgetView)serviceProvider.GetRequiredService(widgetViewType);
         widgetView.WidgetViewModel = widgetViewModel;
         return widgetView;
      }

      return null;
   }

   public IWidgetViewModel? CreateWidgetViewModel(string displayName)
   {
      if (WidgetNameRegistrations.TryGetValue(displayName, out var widgetViewModelType))
      {
         var widgetViewModel = (IWidgetViewModel)serviceProvider.GetRequiredService(widgetViewModelType);
         return widgetViewModel;
      }

      return null;
   }
}