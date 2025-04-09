using System.Globalization;
using WeatherClient.Models;

namespace WeatherClient.Converters;

internal class WeatherConditionToImageConverter : IValueConverter
{
   public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
   {
      if (value is WeatherType weatherType)
      {
         return weatherType switch
         {
            WeatherType.Sunny => ImageSource.FromFile("sunny.png"),
            WeatherType.Cloudy => ImageSource.FromFile("cloud.png"),
            _ => ImageSource.FromFile("question.png")
         };
      }

      return ImageSource.FromFile("question.png");
   }

   public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
      throw new NotImplementedException();
}