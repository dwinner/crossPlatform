using CommunityToolkit.Mvvm.ComponentModel;
using Recipes.Client.Core.Navigation;

namespace Recipes.Client.Core.ViewModels;

public class PickLanguageViewModel : ObservableObject, INavigationParameterReceiver
{
   private readonly INavigationService _navigationService;
   private string _selectedLanguage = null!;

   public PickLanguageViewModel(INavigationService navigationService) => _navigationService = navigationService;

   public string SelectedLanguage
   {
      get => _selectedLanguage;
      set
      {
         if (SetProperty(ref _selectedLanguage, value))
         {
            LanguagePicked();
         }
      }
   }

   // TOREFACTOR: Had better store language list in a separated type
   public List<string> Languages { get; set; } =
   [
      "en-US",
      "fr-FR"
   ];

   public Task OnNavigatedTo(Dictionary<string, object> parameters)
   {
      // FIXME: It's not safety enough to have a string as a map key
      _selectedLanguage = parameters["language"] as string
                          ?? Languages[0];
      OnPropertyChanged(nameof(SelectedLanguage));
      return Task.CompletedTask;
   }

   // FIXME: It's not safety enough to have a string as a map key
   private void LanguagePicked() =>
      _navigationService.GoBackAndReturn(new Dictionary<string, object>
      {
         { "SelectedLanguage", SelectedLanguage }
      });
}