using System.Globalization;

namespace Recipes.Mobile.Converters;

public class RatingAndReviewsToColorConverter : IMultiValueConverter
{
   public object? Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
   {
      var isBackground = parameter is string param
                         && param.Equals("background", StringComparison.CurrentCultureIgnoreCase);
      var hex = isBackground ? "#F2F2F2" : "#EBEBEB";
      if (values is [>= 3, double rating])
      {
         hex = rating switch
         {
            > 0 and < 1.4 => isBackground ? "#E0F7FA" : "#ADD8E6",
            < 2.4 => isBackground ? "#F0C085" : "#CD7F32",
            < 3.5 => isBackground ? "#E5E5E5" : "#C0C0C0",
            <= 4.0 => isBackground ? "#FFF9D6" : "#FFD700",
            _ => null
         };
      }

      return hex is null ? null : Color.FromArgb(hex);
   }

   public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) =>
      throw new NotImplementedException();
}