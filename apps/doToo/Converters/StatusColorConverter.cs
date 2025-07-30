using System.Diagnostics;
using System.Globalization;

namespace DoToo.Converters;

internal sealed class StatusColorConverter : IValueConverter
{
   public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
   {
      var app = Application.Current;
      var isCompleted = (bool)(value ?? false);
      Debug.Assert(app != null, nameof(app) + " != null");
      return (Color)app.Resources[isCompleted ? "CompletedColor" : "ActiveColor"];
   }

   public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => null;
}