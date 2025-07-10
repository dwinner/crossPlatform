namespace c3_DarkAndLightThemes;

public class ThemeInfo
{
   public static readonly ThemeInfo System = new(AppTheme.Unspecified, nameof(System));
   public static readonly ThemeInfo Light = new(AppTheme.Light, nameof(Light));
   public static readonly ThemeInfo Dark = new(AppTheme.Dark, nameof(Dark));

   private ThemeInfo(AppTheme theme, string caption)
   {
      AppTheme = theme;
      Caption = caption;
   }

   public AppTheme AppTheme { get; }
   
   public string Caption { get; }
}