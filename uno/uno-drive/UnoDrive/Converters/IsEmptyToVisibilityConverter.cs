using Microsoft.UI.Xaml.Data;

namespace UnoDrive.Converters;

public class IsEmptyToVisibilityConverter : IValueConverter
{
   public Visibility IsEmpty { get; set; }

   public Visibility IsNotEmpty { get; set; }

   public object Convert(object value, Type targetType, object parameter, string language) =>
      value is string message
         ? string.IsNullOrEmpty(message)
            ? IsEmpty
            : IsNotEmpty
         : Visibility.Collapsed;

   public object ConvertBack(object value, Type targetType, object parameter, string language) =>
      throw new NotImplementedException();
}
