namespace c8_DebugVsRelease;

public partial class AppShell
{
   public AppShell()
   {
      InitializeComponent();
      Routing.RegisterRoute(nameof(TestPage), typeof(TestPage));
   }
}