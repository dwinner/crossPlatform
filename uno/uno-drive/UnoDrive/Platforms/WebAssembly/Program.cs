using System.Diagnostics;
using Uno.UI.Hosting;

namespace UnoDrive;

public class Program
{
   public static async Task Main(string[] args)
   {
      var host = UnoPlatformHostBuilder.Create()
         .App(() => new App())
         .UseWebAssembly()
         .Build();

      await host.RunAsync().ConfigureAwait(true);
      Debug.WriteLine("Web assembly started");
   }
}
