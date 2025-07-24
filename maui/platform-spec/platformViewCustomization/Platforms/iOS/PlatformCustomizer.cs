using UIKit;

namespace c7_CustomizeHandler;

public static partial class PlatformCustomizer
{
   public static partial void CustomizeEntry(object platformView)
   {
      var editor = (UITextField)platformView;
      editor.TintColor = UIColor.FromRGBA(0, 255, 209, 255);
   }
}