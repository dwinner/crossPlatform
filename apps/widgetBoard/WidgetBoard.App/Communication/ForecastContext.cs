using System.Text.Json.Serialization;

namespace WidgetBoard.App.Communication;

[JsonSerializable(typeof(Forecast))]
internal partial class ForecastContext : JsonSerializerContext
{
}