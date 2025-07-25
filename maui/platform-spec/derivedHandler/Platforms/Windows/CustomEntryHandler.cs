using Microsoft.Maui.Platform;
using SolidColorBrush = Microsoft.UI.Xaml.Media.SolidColorBrush;

namespace c7_DerivedHandler;

public partial class CustomEntryHandler
{
   static partial void MapSelectionColor(CustomEntryHandler handler, CustomEntry entry)
   {
      if (handler.PlatformView != null)
      {
         handler.PlatformView.SelectionHighlightColor = new SolidColorBrush(entry.SelectionColor.ToWindowsColor());
      }
   }
}