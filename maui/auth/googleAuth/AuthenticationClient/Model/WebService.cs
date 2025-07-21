using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using c5_AuthenticationClient.Model;

namespace c5_AuthenticationClient;

public class WebService
{
   private static readonly string BaseAddress = "https://YOUR.DEV.TUNNEL.ADDRESS/";
   private readonly HttpClient _httpClient = new()
   {
      BaseAddress = new Uri(BaseAddress)
   };
   public static WebService Instance { get; } = new();

   public async Task<BearerTokenInfo> Authenticate(string email, string password) =>
      await RequestTokenAsync("login/", new { email, password });

   private async Task<BearerTokenInfo> RequestTokenAsync(string url, object postContent)
   {
      var response = await _httpClient.PostAsync(url,
         new StringContent(JsonSerializer.Serialize(postContent), Encoding.UTF8,
            "application/json"));
      response.EnsureSuccessStatusCode();
      var tokenInfo = await response.Content.ReadFromJsonAsync<BearerTokenInfo>();
      SetAuthHeader(tokenInfo.AccessToken);
      return tokenInfo;
   }

   public void SetAuthHeader(string token)
   {
      _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
   }

   public async Task<IEnumerable<User>> GetUsersAsync()
   {
      var response = await _httpClient.GetAsync("users");
      response.EnsureSuccessStatusCode();
      var json = await response.Content.ReadAsStringAsync();
      return JsonSerializer.Deserialize<List<User>>(json, new JsonSerializerOptions
      {
         PropertyNameCaseInsensitive = true
      });
   }

   public async Task<bool> CanDeleteUsersAsync()
   {
      var response = await _httpClient.GetAsync("users/candelete");
      response.EnsureSuccessStatusCode();
      var json = await response.Content.ReadAsStringAsync();
      return JsonSerializer.Deserialize<bool>(json);
   }

   public async Task DeleteUserAsync(string email)
   {
      var response = await _httpClient.DeleteAsync($"users/{email}");
      response.EnsureSuccessStatusCode();
   }

   public async Task<User> GetCurrentUserAsync()
   {
      var response = await _httpClient.GetAsync("me");
      response.EnsureSuccessStatusCode();
      var json = await response.Content.ReadAsStringAsync();
      return JsonSerializer.Deserialize<User>(json, new JsonSerializerOptions
      {
         PropertyNameCaseInsensitive = true
      });
   }

   public async Task GoogleAuthAsync()
   {
      var authResult = await WebAuthenticator.Default.AuthenticateAsync(
         new Uri($"{BaseAddress}mauth/google"),
         new Uri("myapp://"));
      var tokenInfo = new BearerTokenInfo
      {
         AccessToken = authResult.AccessToken,
         RefreshToken = authResult.RefreshToken,
         ExpiresIn = int.Parse(authResult.Properties["expires_in"]),
         TokenTimestamp = DateTime.UtcNow
      };
      SetAuthHeader(tokenInfo.AccessToken);
   }
}