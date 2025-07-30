using Recipes.L10N;
using Recipes.Mobile.Navigation;

namespace Recipes.Mobile;

public partial class App
{
   public App(INavigationInterceptor interceptor, ILocalizationManager l10NManager)
   {
      l10NManager.RestorePreviousCulture();
      if (Current != null)
      {
         Current.UserAppTheme = AppTheme.Light;
      }

      InitializeComponent();
      MainPage = new AppShell(interceptor);
   }
}