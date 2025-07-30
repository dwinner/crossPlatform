using System.Globalization;

namespace Recipes.Mobile.Converters;

public class RatingToColorConverter : IValueConverter
{
   public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
   {
      var isBackground = parameter is string param
                         && param.Equals("background", StringComparison.CurrentCultureIgnoreCase);
      var hex = value switch
      {
         double and > 0 and < 1.4 => isBackground ? "#E0F7FA" : "#ADD8E6", //blue
         < 2.4 => isBackground ? "#F0C085" : "#CD7F32", //bronze
         < 3.5 => isBackground ? "#E5E5E5" : "#C0C0C0", //silver
         <= 4.0 => isBackground ? "#FFF9D6" : "#FFD700", //gold
         _ => "#EBEBEB" // Default color if rating is outside the expected range
      };

      return Color.FromArgb(hex);
   }

   public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
      throw new NotImplementedException();
}