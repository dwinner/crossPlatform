using System.Globalization;

namespace DoToo.Converters;

internal sealed class FilterTextConverter : IValueConverter
{
   public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
   {
      var showAll = (bool)(value ?? false);
      return showAll ? "All" : "Active";
   }

   public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => null;
}