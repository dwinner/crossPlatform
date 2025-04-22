using System.Text.Json.Serialization;

namespace News.Models;

public class Article
{
   [JsonPropertyName("source")]
   public NewsSource? NewsSource { get; set; }

   [JsonPropertyName("author")]
   public string Author { get; set; } = string.Empty;

   [JsonPropertyName("title")]
   public string Title { get; set; } = string.Empty;

   [JsonPropertyName("description")]
   public string Description { get; set; } = string.Empty;

   [JsonPropertyName("url")]
   public string Url { get; set; } = string.Empty;

   [JsonPropertyName("urlToImage")]
   public string UrlToImage { get; set; } = string.Empty;

   [JsonPropertyName("publishedAt")]
   public DateTime PublishedAt { get; set; }

   [JsonPropertyName("content")]
   public string Content { get; set; } = string.Empty;
}