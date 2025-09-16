using System.Globalization;

namespace WidgetBoard.App.Converters;

public sealed class IsEqualToStateConverter : IValueConverter
{
   public State State { get; set; }

   public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
      value is State state && state == State;

   public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
      throw new NotImplementedException();
}