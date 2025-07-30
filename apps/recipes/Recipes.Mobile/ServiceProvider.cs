namespace Recipes.Mobile;

public static class ServiceProvider
{
   public static IServiceProvider Current
      =>
#if WINDOWS10_0_17763_0_OR_GREATER
    MauiWinUIApplication.Current.Services;
#elif ANDROID
    MauiApplication.Current.Services;
#elif IOS || MACCATALYST
    MauiUIApplicationDelegate.Current.Services;
#else
         throw new NotSupportedException("Not supported platform");
#endif
   public static TService GetService<TService>()
#pragma warning disable CS8603 // Possible null reference return.
      => Current.GetService<TService>();
#pragma warning restore CS8603 // Possible null reference return.
}