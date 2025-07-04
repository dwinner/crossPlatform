using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace WorkingWithWebApi.Services;

internal sealed class WebApiService
{
   public static async Task<HttpResponseMessage> GetDataAsync(string url, string id = null)
   {
      HttpResponseMessage response;
      try
      {
         using var client = new HttpClient();
         
         // Add your Bearer token here...
         //client.DefaultRequestHeaders.Authorization =
         //    new AuthenticationHeaderValue("Bearer", "Your Oauth token");
         // Use domain authentication...
         //var client = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true });

         if (id != null)
         {
            url = $"{url}/{id}";
         }

         response = await client.GetAsync(url);
      }
      catch (HttpRequestException)
      {
         response = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
      }
      catch
      {
         response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
      }

      return response;
   }

   public static async Task<HttpResponseMessage> WriteDataAsync<T>(T data, string url, string id = null)
   {
      HttpResponseMessage response;
      try
      {
         using var client = new HttpClient();
         client.DefaultRequestHeaders.Accept.Clear();
         client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
         var json = JsonConvert.SerializeObject(data);
         var content = new StringContent(json, Encoding.UTF8, "application/json");

         if (id != null)
         {
            url = $"{url}/{id}";
         }

         response = await client.PostAsync(url, content);
      }
      catch (HttpRequestException)
      {
         response = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
      }
      catch
      {
         response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
      }

      return response;
   }

   public static async Task<HttpResponseMessage> DeleteDataAsync(string url, int id)
   {
      HttpResponseMessage response;
      try
      {
         using var client = new HttpClient();
         var fullUri = $"{url}/{id}";
         response = await client.DeleteAsync(fullUri);
      }
      catch (HttpRequestException)
      {
         response = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
      }
      catch
      {
         response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
      }

      return response;
   }
}