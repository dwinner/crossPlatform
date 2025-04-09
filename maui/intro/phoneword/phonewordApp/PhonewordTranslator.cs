using System.Text;

namespace phonewordApp;

public static class PhonewordTranslator
{
   private static readonly string[] Digits =
   [
      "ABC",
      "DEF",
      "GHI",
      "JKL",
      "MNO",
      "PQRS",
      "TUV",
      "WXYZ"
   ];

   public static string? ToNumber(string raw)
   {
      if (string.IsNullOrWhiteSpace(raw))
      {
         return null;
      }

      raw = raw.ToUpperInvariant();
      var newNumber = new StringBuilder();
      foreach (var chr in raw)
      {
         if (" -0123456789".Contains(chr))
         {
            newNumber.Append(chr);
         }
         else
         {
            var result = TranslateToNumber(chr);
            if (result != null)
            {
               newNumber.Append(result);
            }
            else
            {
               return null;
            }
         }
      }

      return newNumber.ToString();
   }

   private static bool Contains(this string keyString, char chr) => keyString.IndexOf(chr) >= 0;

   private static int? TranslateToNumber(char chr)
   {
      for (var i = 0; i < Digits.Length; i++)
      {
         if (Digits[i].Contains(chr))
         {
            return 2 + i;
         }
      }

      return null;
   }
}