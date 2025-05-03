namespace SwiperApp.Core.Utils;

internal class Picture
{
   //private static readonly string _UriSource = $"https://picsum.photos/400/400/?random&ts={DateTime.Now.Ticks}";
   private static readonly string[] _JpgPictures;
   private static readonly Random _Random = new();

   static Picture() => _JpgPictures = Directory.EnumerateFiles(".", "*.jpg").ToArray();

   public Picture()
   {
      //Uri = new Uri(_UriSource);
      var rndIndex = _Random.Next(0, _JpgPictures.Length - 1);
      FileName = _JpgPictures[rndIndex];
      var generator = new DescriptionGenerator();
      Description = generator.Generate();
   }

   //public Uri Uri { get; init; }

   public string Description { get; init; }

   public string FileName { get; init; }
}