using Microsoft.UI.Xaml.Controls;
using Color = Windows.UI.Color;
using SolidColorBrush = Microsoft.UI.Xaml.Media.SolidColorBrush;

namespace c7_CustomizeHandler;

public static partial class PlatformCustomizer
{
   public static partial void CustomizeEntry(object platformView)
   {
      var editor = (TextBox)platformView;
      editor.SelectionHighlightColor = new SolidColorBrush(Color.FromArgb(255, 0, 255, 209));
   }
}