using Gallery.Core;
using Gallery.Core.Services;
using gallery.Mac;

namespace Gallery.Mac;

public static class MauiProgram
{
   public static MauiApp CreateMauiApp()
   {
      var builder = MauiApp.CreateBuilder();
      builder.Services.AddSingleton<IPhotoImporter>(_ => new MacPhotoImporter());
      builder.UseSharedMauiApp();

      return builder.Build();
   }
}