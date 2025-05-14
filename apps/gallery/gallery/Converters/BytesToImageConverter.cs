using System.Globalization;

namespace Gallery.Core.Converters;

public class BytesToImageConverter : IValueConverter
{
   public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
   {
      if (value != null)
      {
         var bytes = (byte[])value;
         var imgStream = new MemoryStream(bytes);
         return ImageSource.FromStream(() => imgStream);
      }

      return null;
   }

   public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
      throw new NotImplementedException();
}