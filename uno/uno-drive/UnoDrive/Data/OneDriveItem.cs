using LiteDB;

namespace UnoDrive.Data;

public class OneDriveItem
{
   public string Id { get; set; } = string.Empty;

   public string Name { get; set; } = string.Empty;

   public string Path { get; set; } = string.Empty;

   public string PathId { get; set; } = string.Empty;

   public DateTime Modified { get; set; }

   public string FileSize { get; set; } = string.Empty;

   public OneDriveItemType Type { get; set; }

   public string ThumbnailPath { get; set; } = string.Empty;

   [BsonIgnore]
   public ImageSource? ThumbnailSource { get; set; }
}
