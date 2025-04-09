using System.Globalization;
using System.Resources;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Recipes.L10N;

// TOREFACTOR: Strange decision to have explicitly declared singleton

public class LocalizedResourcesProvider : ObservableObject, ILocalizedResourcesProvider
{
   private readonly ResourceManager _resourceManager;
   private CultureInfo _currentCulture;

   public LocalizedResourcesProvider(ResourceManager resourceManager)
   {
      _resourceManager = resourceManager;
      _currentCulture = CultureInfo.CurrentUICulture;
      Instance = this;
   }

   public static LocalizedResourcesProvider Instance { get; private set; }

   public string this[string key] => _resourceManager.GetString(key, _currentCulture) ?? key;

   public void UpdateCulture(CultureInfo cultureInfo)
   {
      _currentCulture = cultureInfo;

      // Raise property change event on indexer parameter
      OnPropertyChanged("Item");
   }
}