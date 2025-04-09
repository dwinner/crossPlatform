using System.Globalization;

namespace Recipes.L10N;

/// <summary>
///    Contract to control switching between languages
/// </summary>
public interface ILocalizationManager
{
   /// <summary>
   ///    Restore the previous culture
   /// </summary>
   /// <param name="defaultCulture">Default culture to pass if present</param>
   void RestorePreviousCulture(CultureInfo? defaultCulture = null);

   /// <summary>
   ///    Change the user's culture
   /// </summary>
   /// <param name="cultureInfo">Target culture info</param>
   void UpdateUserCulture(CultureInfo cultureInfo);

   /// <summary>
   ///    Get the current user culture
   /// </summary>
   /// <param name="defaultCulture">Default culture to pass if present</param>
   /// <returns>The current user's culture info</returns>
   CultureInfo GetUserCulture(CultureInfo? defaultCulture = null);
}