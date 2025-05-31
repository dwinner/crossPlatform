using Gallery.Core;
using Gallery.Core.Services;
using gallery.iOS;

namespace Gallery.iOS;

public static class MauiProgram
{
   public static MauiApp CreateMauiApp()
   {
      var builder = MauiApp.CreateBuilder();
      builder.Services.AddSingleton<IPhotoImporter>(_ => new IosPhotoImporter());
      builder.UseSharedMauiApp();

      return builder.Build();
   }
}