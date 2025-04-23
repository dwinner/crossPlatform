using SwiperApp.Core;

namespace SwiperApp.WinUI;

public static class MauiProgram
{
   public static MauiApp CreateMauiApp()
   {
      var builder = MauiApp.CreateBuilder();
      builder
         .UseSharedMauiApp();

      return builder.Build();
   }
}