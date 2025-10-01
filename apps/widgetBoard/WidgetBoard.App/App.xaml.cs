namespace WidgetBoard.App;

public partial class App
{
   private readonly AppShell _appShell;

   public App(AppShell appShell)
   {
      _appShell = appShell;
      InitializeComponent();
   }

   protected override Window CreateWindow(IActivationState? activationState) => new(_appShell);
}