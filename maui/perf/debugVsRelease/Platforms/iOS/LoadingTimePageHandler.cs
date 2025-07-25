using Microsoft.Maui.Platform;
using ContentView = Microsoft.Maui.Platform.ContentView;

namespace c8_DebugVsRelease;

public partial class LoadingTimePageHandler
{
   protected override ContentView CreatePlatformView()
   {
      ViewController ??= new CustomViewController(VirtualView, MauiContext);
      if (ViewController is PageViewController { CurrentPlatformView: ContentView platformView })
      {
         return platformView;
      }

      if (ViewController.View is ContentView contentView)
      {
         return contentView;
      }

      throw new InvalidOperationException(
         $"PageViewController.View must be a {nameof(ContentView)}");
   }

   private class CustomViewController(IView page, IMauiContext mauiCtx) : PageViewController(page, mauiCtx)
   {
      public override void ViewDidAppear(bool animated)
      {
         base.ViewDidAppear(animated);
         LoadingTimePage.ShowTimeElapsed();
      }
   }
}