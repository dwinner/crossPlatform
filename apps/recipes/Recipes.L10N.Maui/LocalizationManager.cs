using System.Globalization;

namespace Recipes.L10N.Maui;

public class LocalizationManager : ILocalizationManager
{
   private readonly ILocalizedResourcesProvider _resourceProvider;
   private CultureInfo? _currentCulture;

   public LocalizationManager(ILocalizedResourcesProvider resourceProvider) => _resourceProvider = resourceProvider;

   public void RestorePreviousCulture(CultureInfo? defaultCulture = null)
   {
      SetCulture(GetUserCulture(defaultCulture));
   }

   public void UpdateUserCulture(CultureInfo cultureInfo)
   {
      // FIXME: Move hardcoded string from there
      Preferences.Default.Set("UserCulture", cultureInfo.Name);
      SetCulture(cultureInfo);
   }

   public CultureInfo GetUserCulture(CultureInfo? defaultCulture = null)
   {
      if (_currentCulture is not null)
      {
         return _currentCulture;
      }

      // FIXME: Move hardcoded string from there
      var culture = Preferences.Default.Get("UserCulture", string.Empty);
      _currentCulture = string.IsNullOrEmpty(culture)
         ? defaultCulture ?? CultureInfo.CurrentCulture
         : new CultureInfo(culture);

      return _currentCulture;
   }

   private void SetCulture(CultureInfo cultureInfo)
   {
      _currentCulture = cultureInfo;
      Application.Current?.Dispatcher.Dispatch(() =>
      {
         CultureInfo.CurrentCulture = cultureInfo;
         CultureInfo.CurrentUICulture = cultureInfo;
         CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
         CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
      });
      _resourceProvider.UpdateCulture(cultureInfo);
   }
}