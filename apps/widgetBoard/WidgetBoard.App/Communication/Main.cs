using System.Text.Json.Serialization;

namespace WidgetBoard.App.Communication;

public class Main
{
   [JsonPropertyName("temp")]
   public double Temperature { get; set; }
}