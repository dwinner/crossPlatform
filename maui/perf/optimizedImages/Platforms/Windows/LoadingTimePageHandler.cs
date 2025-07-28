using Microsoft.Maui.Platform;

namespace c8_DebugVsRelease;

public partial class LoadingTimePageHandler
{
   protected override void ConnectHandler(ContentPanel platformView)
   {
      base.ConnectHandler(platformView);
      if (platformView is { } contentPanel)
      {
         contentPanel.Loaded += (s, e) => LoadingTimePage.ShowTimeElapsed();
      }
   }
}