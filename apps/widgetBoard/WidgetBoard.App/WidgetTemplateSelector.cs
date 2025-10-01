using WidgetBoard.App.ViewModels;

namespace WidgetBoard.App;

public class WidgetTemplateSelector(WidgetFactory widgetFactory) : DataTemplateSelector
{
   protected override DataTemplate? OnSelectTemplate(object item, BindableObject container)
   {
      if (item is IWidgetViewModel widgetViewModel)
      {
         return new DataTemplate(() => widgetFactory.CreateWidget(widgetViewModel));
      }

      return null;
   }
}