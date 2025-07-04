namespace People.WinUI;

public partial class App
{
   /// <summary>
   ///    Initializes the singleton application object.  This is the first line of authored code
   ///    executed, and as such is the logical equivalent of main() or WinMain().
   /// </summary>
   public App()
   {
      InitializeComponent();
   }

   protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}