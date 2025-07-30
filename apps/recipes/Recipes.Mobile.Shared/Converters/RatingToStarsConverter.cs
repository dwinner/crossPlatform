using System.Globalization;

namespace Recipes.Mobile.Shared.Converters;

public class RatingToStarsConverter : IValueConverter
{
   private const string FullStar = "\ue838";
   private const string HalfStar = "\ue839";

   public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
   {
      if (value is not double rating || rating < 0 || rating > 5)
      {
         return string.Empty;
      }

      var fullStars = (int)rating;
      var hasHalfStar = rating % 1 >= 0.5;

      return string.Concat(
         string.Join(string.Empty, Enumerable.Repeat(FullStar, fullStars)), hasHalfStar ? HalfStar : string.Empty);
   }

   public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
      throw new NotImplementedException();
}