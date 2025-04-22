using System.Text.Json.Serialization;

namespace News.Models;

public class NewsSource
{
   [JsonPropertyName("id")]
   public string Id { get; set; } = string.Empty;

   [JsonPropertyName("name")]
   public string Name { get; set; } = string.Empty;
}