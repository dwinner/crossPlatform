namespace SwiperApp.Core.Utils;

internal class DescriptionGenerator
{
   private static readonly Random _Random = new();
   private readonly string[] _adjectives = ["nice", "horrible", "great", "terribly old", "brand new"];
   private readonly string[] _other = ["picture of grandpa", "car", "photo of a forest", "duck"];

   public string Generate()
   {
      var adj = _adjectives[_Random.Next(_adjectives.Count())];
      var phrase = _other[_Random.Next(_other.Count())];
      return $"A {adj} {phrase}";
   }
}