using System.Text.Json.Serialization;

namespace Weather.Models;

public class Main
{
   [JsonPropertyName("temp")]
   public double Temp { get; set; }

   [JsonPropertyName("temp_min")]
   public double TempMin { get; set; }

   [JsonPropertyName("temp_max")]
   public double TempMax { get; set; }

   [JsonPropertyName("pressure")]
   public double Pressure { get; set; }

   [JsonPropertyName("sea_level")]
   public double SeaLevel { get; set; }

   [JsonPropertyName("grnd_level")]
   public double GrndLevel { get; set; }

   [JsonPropertyName("humidity")]
   public int Humidity { get; set; }

   [JsonPropertyName("temp_kf")]
   public double TempKf { get; set; }
}

public class Weather
{
   [JsonPropertyName("id")]
   public int Id { get; set; }

   [JsonPropertyName("main")]
   public string Main { get; set; }

   [JsonPropertyName("description")]
   public string Description { get; set; }

   [JsonPropertyName("icon")]
   public string Icon { get; set; }
}

public class Clouds
{
   [JsonPropertyName("all")]
   public int All { get; set; }
}

public class Wind
{
   [JsonPropertyName("speed")]
   public double Speed { get; set; }

   [JsonPropertyName("deg")]
   public double Deg { get; set; }
}

public class Rain;

public class Sys
{
   [JsonPropertyName("pod")]
   public string Pod { get; set; }
}

public class List
{
   [JsonPropertyName("dt")]
   public long Dt { get; set; }

   [JsonPropertyName("main")]
   public Main Main { get; set; }

   [JsonPropertyName("weather")]
   public List<Weather> Weather { get; set; }

   [JsonPropertyName("clouds")]
   public Clouds Clouds { get; set; }

   [JsonPropertyName("wind")]
   public Wind Wind { get; set; }

   [JsonPropertyName("rain")]
   public Rain Rain { get; set; }

   [JsonPropertyName("sys")]
   public Sys Sys { get; set; }

   [JsonPropertyName("dt_txt")]
   public string DtTxt { get; set; }
}

public class Coord
{
   [JsonPropertyName("lat")]
   public double Lat { get; set; }

   [JsonPropertyName("lon")]
   public double Lon { get; set; }
}

public class City
{
   [JsonPropertyName("id")]
   public int Id { get; set; }

   [JsonPropertyName("name")]
   public string Name { get; set; }

   [JsonPropertyName("coord")]
   public Coord Coord { get; set; }

   [JsonPropertyName("country")]
   public string Country { get; set; }
}

public class WeatherData
{
   [JsonPropertyName("cod")]
   public string Cod { get; set; }

   [JsonPropertyName("message")]
   public double Message { get; set; }

   [JsonPropertyName("cnt")]
   public int Cnt { get; set; }

   [JsonPropertyName("list")]
   public List<List> List { get; set; }

   [JsonPropertyName("city")]
   public City City { get; set; }
}