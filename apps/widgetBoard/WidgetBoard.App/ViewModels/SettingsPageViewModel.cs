using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WidgetBoard.App.Data;

namespace WidgetBoard.App.ViewModels;

public partial class SettingsPageViewModel : ViewModelBase
{
   private readonly IPreferences _preferences;
   private readonly ISecureStorage _secureStorage;

   [ObservableProperty] private string _lastUsedBoard = string.Empty;

   [ObservableProperty] private string _openWeatherApiToken = string.Empty;

   public SettingsPageViewModel(
      IPreferences preferences, IBoardRepository boardRepository, ISecureStorage secureStorage)
   {
      _preferences = preferences;
      _secureStorage = secureStorage;

      var lastUsedBoardId = _preferences.Get("LastUsedBoardId", -1);
      if (lastUsedBoardId != -1)
      {
         LastUsedBoard = boardRepository.LoadBoard(lastUsedBoardId)?.Name ?? string.Empty;
      }

      OpenWeatherApiToken = _secureStorage.GetAsync("OpenWeatherApiToken").GetAwaiter().GetResult()
                            ?? string.Empty;
   }

   [RelayCommand]
   private async Task SaveApiToken()
   {
      await _secureStorage.SetAsync("OpenWeatherApiToken", OpenWeatherApiToken);
   }

   [RelayCommand]
   private void ClearLastUsedBoard()
   {
      _preferences.Remove("LastUsedBoardId");
      LastUsedBoard = string.Empty;
   }
}