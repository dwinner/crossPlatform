namespace MeTracker;

public partial class App
{
   public App()
   {
      InitializeComponent();
   }

   protected override Window CreateWindow(IActivationState? activationState) => new(new AppShell());

   protected override void OnResume()
   {
      Windows[0].Page = new AppShell();
   }
}