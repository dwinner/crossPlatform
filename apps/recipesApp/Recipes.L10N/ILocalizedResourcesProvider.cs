using System.Globalization;

namespace Recipes.L10N;

/// <summary>
///    Provider for translation
/// </summary>
public interface ILocalizedResourcesProvider
{
   /// <summary>
   ///    Key to retrieve the translation
   /// </summary>
   /// <param name="key">Key for translation</param>
   /// <returns></returns>
   string this[string key] { get; }

   /// <summary>
   ///    Update the culture info
   /// </summary>
   /// <param name="cultureInfo">The culture info</param>
   void UpdateCulture(CultureInfo cultureInfo);
}