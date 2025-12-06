namespace UnoDrive.Logging;

public static class LoggingExtensions
{
   public static void AddLoggingForUnoDrive(this IServiceCollection services)
   {
      var configuration = new LoggingConfiguration();
      configuration.ConfigureLogging(services);
   }
}
