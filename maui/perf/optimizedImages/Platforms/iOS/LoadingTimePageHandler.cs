using System.Diagnostics;
using Microsoft.Maui.Platform;
using ContentView = Microsoft.Maui.Platform.ContentView;

namespace c8_DebugVsRelease;

public partial class LoadingTimePageHandler
{
   protected override ContentView CreatePlatformView()
   {
      Debug.Assert(MauiContext != null, $"{nameof(MauiContext)} != null");
      ViewController ??= new CustomViewController(VirtualView, MauiContext);
      if (ViewController is PageViewController { CurrentPlatformView: ContentView view })
      {
         return view;
      }

      if (ViewController.View is ContentView platformView)
      {
         return platformView;
      }

      throw new InvalidOperationException($"PageViewController.View must be a {nameof(ContentView)}");
   }

   private class CustomViewController(IView page, IMauiContext mauiContext)
      : PageViewController(page, mauiContext)
   {
      public override void ViewDidAppear(bool animated)
      {
         base.ViewDidAppear(animated);
         LoadingTimePage.ShowTimeElapsed();
      }
   }
}