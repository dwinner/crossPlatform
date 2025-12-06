#if __IOS__
namespace UnoDrive.Logging;

public partial class LoggingConfiguration
{
   public partial void ConfigureLogging(IServiceCollection services)
   {
      services.AddLogging(builder =>
      {
         builder
            .ClearProviders()
            .AddProvider(new OSLogLoggerProvider())
#if DEBUG
            .AddFilter("UnoDrive", LogLevel.Information)
#else
				.AddFilter("UnoDrive", LogLevel.Debug)
#endif
            .AddFilter("Uno", LogLevel.Debug)
            .AddFilter("Windows", LogLevel.Debug)
            .AddFilter("Microsoft", LogLevel.Debug)
            .AddDebug();
      });
   }
}
#endif
