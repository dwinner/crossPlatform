namespace c8_DebugVsRelease;

public partial class App
{
   public App()
   {
      InitializeComponent();
   }

   protected override Window CreateWindow(IActivationState activationState) => new(new AppShell());
}