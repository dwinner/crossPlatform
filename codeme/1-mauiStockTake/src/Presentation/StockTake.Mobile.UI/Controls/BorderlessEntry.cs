namespace MauiStockTake.UI.Controls;
public class BorderlessEntry : Entry
{
	public BorderlessEntry()
	{
        ModifyEntry();
    }

    private void ModifyEntry()
	{
        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("RemoveBorder", (handler, view) =>
        {
            if (view is BorderlessEntry)
            {

#if ANDROID
                handler.PlatformView.Background = null;
                handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#elif WINDOWS
                handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
                handler.PlatformView.Background = null;
                handler.PlatformView.FocusVisualMargin = new Microsoft.UI.Xaml.Thickness(0);
#elif IOS
                handler.PlatformView.BackgroundColor = UIKit.UIColor.Clear;
                handler.PlatformView.Layer.BorderWidth = 0;
                handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#endif
            }
        });
    }
}
