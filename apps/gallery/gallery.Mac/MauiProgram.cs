using Gallery.Core;

namespace gallery.Mac;

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