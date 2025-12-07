using System.Threading.Tasks;
using Avalonia;
using Avalonia.Browser;
using Panels;
using ReactiveUI.Avalonia;

internal sealed class Program
{
   private static Task Main(string[] args) => BuildAvaloniaApp()
      .WithInterFont()
      .UseReactiveUI()
      .StartBrowserAppAsync("out");

   public static AppBuilder BuildAvaloniaApp()
      => AppBuilder.Configure<App>();
}