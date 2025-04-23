using SwiperApp.Core;

namespace Swiper.Droid;

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