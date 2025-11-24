using StockTake.Mobile.UI.Helpers;
using StockTake.Mobile.UI.Pages;
using StockTake.Mobile.UI.Services;

namespace StockTake.Mobile.UI;

public partial class App : Application
{
   private readonly IAuthService _authService;
   private bool _loggedIn;

   public static Theme Theme { get; set; } = Theme.Default;
   
   public App(IAuthService authService)
   {
      _authService = authService;
      InitializeComponent();
   }

   protected override Window CreateWindow(IActivationState? activationState)
   {
      return new Window(new AppShell());
   }
   
   protected override async void OnStart()
   {
      base.OnStart();

      if (!_loggedIn)
      {
         await MainPage.Navigation.PushModalAsync(new LoginPage(_authService));
         _loggedIn = true;
      }
   }
}