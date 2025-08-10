using System.Text.Json;
using SticksAndStones.Models;

namespace SticksAndStones.Services;

public class Settings
{
   private readonly JsonSerializerOptions jsonSerializerOptions = new(JsonSerializerDefaults.Web);
   private const string LastPlayerKey = nameof(LastPlayerKey);
   private const string ServerUrlKey = nameof(ServerUrlKey);

#if DEBUG && ANDROID
    private const string ServerUrlDefault = "http://10.0.2.2:7071/api";
#else
   private const string ServerUrlDefault = "http://localhost:7071/api";
#endif

   public string ServerUrl
   {
      get => Preferences.ContainsKey(ServerUrlKey)
         ? Preferences.Get(ServerUrlKey, ServerUrlDefault)
         : ServerUrlDefault;
      set => Preferences.Set(ServerUrlKey, value);
   }

   public Player LastPlayer
   {
      get
      {
         if (Preferences.ContainsKey(LastPlayerKey))
         {
            var playerJson = Preferences.Get(LastPlayerKey, string.Empty);
            return JsonSerializer.Deserialize<Player>(playerJson, jsonSerializerOptions)
                   ?? new Player();
         }

         return new Player();
      }
      set => Preferences.Set(LastPlayerKey, JsonSerializer.Serialize(value, jsonSerializerOptions));
   }
}