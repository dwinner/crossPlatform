namespace c3_DarkAndLightThemes;

public class ThemeInfo(AppTheme theme, string caption)
{
   public static readonly ThemeInfo System = new(AppTheme.Unspecified, nameof(System));
   public static readonly ThemeInfo Light = new(AppTheme.Light, nameof(Light));
   public static readonly ThemeInfo Dark = new(AppTheme.Dark, nameof(Dark));

   public AppTheme AppTheme { get; } = theme;

   public string Caption { get; } = caption;
}