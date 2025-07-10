namespace c3_DarkAndLightThemes;

public partial class App
{
   public App()
   {
      InitializeComponent();
   }

   protected override Window CreateWindow(IActivationState activationState) => new(new AppShell());
}