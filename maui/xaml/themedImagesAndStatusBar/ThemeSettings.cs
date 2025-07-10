using CommunityToolkit.Mvvm.ComponentModel;

namespace c3_DarkAndLightThemes;

public partial class ThemeSettings : ObservableObject
{
   [ObservableProperty] private ThemeInfo _selectedTheme = ThemesList[0];

   public static List<ThemeInfo> ThemesList { get; } =
   [
      ThemeInfo.System,
      ThemeInfo.Light,
      ThemeInfo.Dark
   ];

   public static ThemeSettings Current { get; } = new();

   partial void OnSelectedThemeChanged(ThemeInfo oldValue, ThemeInfo newValue)
   {
      Application.Current.UserAppTheme = newValue.AppTheme;
   }
}