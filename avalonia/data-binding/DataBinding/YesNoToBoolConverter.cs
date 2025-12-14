using System;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Data.Converters;

namespace DataBinding;

public class YesNoToBoolConverter : IValueConverter
{
   public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
   {
      if (value == null)
      {
         return false;
      }

      var item = value as ComboBoxItem;
      var actualValue = item?.Content?.ToString() ?? string.Empty;
      switch (actualValue)
      {
         case "Yes":
            return true;
         default:
            return false;
      }
   }

   public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
      throw new NotImplementedException();
}