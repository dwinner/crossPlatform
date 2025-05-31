namespace Gallery.Core.Models;

public class Photo
{
   public required string Filename { get; set; }

   public required byte[] Bytes { get; set; }
}