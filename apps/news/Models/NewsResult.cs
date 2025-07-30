using System.Text.Json.Serialization;

namespace News.Models;

public class NewsResult
{
   [JsonPropertyName("status")]
   public string Status { get; set; } = string.Empty;

   [JsonPropertyName("totalResults")]
   public int TotalResults { get; set; }

   [JsonPropertyName("articles")]
   public required List<Article> Articles { get; set; }
}