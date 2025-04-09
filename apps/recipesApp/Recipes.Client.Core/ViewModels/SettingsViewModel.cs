using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Recipes.Client.Core.Messages;
using Recipes.Client.Core.Navigation;
using Recipes.Client.Core.Services;
using Recipes.L10N;

namespace Recipes.Client.Core.ViewModels;

public class SettingsViewModel : ObservableObject, INavigationParameterReceiver
{
   private const string DefaultLang = "en-US";
   private readonly IDialogService _dialogService;
   private readonly ILocalizationManager _localizationManager;
   private readonly INavigationService _navigationService;
   private readonly ILocalizedResourcesProvider _resources;
   private string _currentLanguage;
   private readonly WeakReferenceMessenger _messenger = WeakReferenceMessenger.Default;

   public SettingsViewModel(INavigationService service,
      IDialogService dialogService,
      ILocalizationManager localizationManager,
      ILocalizedResourcesProvider resources)
   {
      _dialogService = dialogService;
      _localizationManager = localizationManager;
      _resources = resources;
      _navigationService = service;
      _currentLanguage = CultureInfo.CurrentCulture.Name;
      SelectLanguageCommand = new AsyncRelayCommand(ChooseLanguage);
   }

   public string CurrentLanguage
   {
      get => _currentLanguage;
      set => SetProperty(ref _currentLanguage, value);
   }

   public AsyncRelayCommand SelectLanguageCommand { get; }

   public Task OnNavigatedTo(Dictionary<string, object> parameters)
   {
      // FIXME: bad idea to pass string as a map key
      if (parameters.TryGetValue("SelectedLanguage", out var parameter))
      {
         var lang = parameter as string ?? DefaultLang;
         return LanguageUpdated(lang);
      }

      return Task.CompletedTask;
   }

   private Task ChooseLanguage() => _navigationService.GoToChooseLanguage(CurrentLanguage);

   private async Task LanguageUpdated(string newLanguage)
   {
      var confirm = await ConfirmSwitchLanguage().ConfigureAwait(true);
      if (confirm)
      {
         SwitchLanguage(newLanguage);
         await NotifySwitch().ConfigureAwait(true);
      }
   }

   private Task<bool> ConfirmSwitchLanguage()
      => _dialogService.AskYesNo(
         _resources["SwitchLanguageDialogTitle"],
         _resources["SwitchLanguageDialogText"],
         _resources["YesDialogButton"],
         _resources["NoDialogButton"]);

   private Task NotifySwitch()
      => _dialogService.Notify(
         _resources["LanguageSwitchedTitle"],
         _resources["LanguageSwitchedText"],
         _resources["OKDialogButton"]);

   private void SwitchLanguage(string newLanguage)
   {
      CurrentLanguage = newLanguage;
      var newCulture = new CultureInfo(newLanguage);
      _localizationManager.UpdateUserCulture(newCulture);
      _messenger.Send(new CultureChangedMessage(newCulture));
   }
}