using IAuthenticationService = UnoDrive.Authentication.IAuthenticationService;

namespace UnoDrive.Services;

public class NavigationService : INavigationService
{
   private readonly IAuthenticationService authentication;

   public NavigationService(IAuthenticationService authentication)
   {
      this.authentication = authentication;
   }

   public void NavigateToDashboard() =>
      GetRootFrame()?.Navigate(typeof(Dashboard), this);

   public async Task SignOutAsync()
   {
      await authentication.SignOutAsync();
      GetRootFrame()?.Navigate(typeof(LoginPage), null);
   }

   private Frame? GetRootFrame()
   {
      var window = ((App)Application.Current).Window;
      if (window.Content is Frame rootFrame)
      {
         return rootFrame;
      }

      return null;
   }
}
