using Microsoft.Maui.Handlers;

#if ANDROID
using Color = Android.Graphics.Color;
#endif

namespace StockTake.Mobile.UI.Controls;

public class BorderlessEntry : Entry
{
   public BorderlessEntry()
   {
      ModifyEntry();
   }

   private void ModifyEntry()
   {
      EntryHandler.Mapper.AppendToMapping("RemoveBorder", (handler, view) =>
      {
         if (view is BorderlessEntry)
         {
#if ANDROID
            handler.PlatformView.Background = null;
            handler.PlatformView.SetBackgroundColor(Color.Transparent);
#elif WINDOWS
                handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
                handler.PlatformView.Background = null;
                handler.PlatformView.FocusVisualMargin = new Microsoft.UI.Xaml.Thickness(0);
#elif IOS || MACCATALYST
                handler.PlatformView.BackgroundColor = UIKit.UIColor.Clear;
                handler.PlatformView.Layer.BorderWidth = 0;
                handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#endif
         }
      });
   }
}