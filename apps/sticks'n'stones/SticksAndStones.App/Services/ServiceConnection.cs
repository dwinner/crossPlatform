using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using SticksAndStones.Models;

namespace SticksAndStones.Services;

public sealed partial class ServiceConnection : IDisposable
{
   private readonly HttpClient httpClient;
   private readonly ILogger logger;
   private readonly JsonSerializerOptions serializerOptions;

   public ServiceConnection(ILogger<ServiceConnection> logger, Settings settings)
   {
      httpClient = new HttpClient
      {
         BaseAddress = new Uri(settings.ServerUrl)
      };
      httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
      serializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);

      this.logger = logger;
   }

   public AsyncLazy<HubConnection> Hub { get; private set; }

   public void Dispose()
   {
      httpClient?.Dispose();
      Hub?.Value?.Dispose();
      GC.SuppressFinalize(this);
   }

   public void ConnectHub(ConnectionInfo config)
   {
      Hub = new AsyncLazy<HubConnection>(async () =>
      {
         var connectionBuilder = new HubConnectionBuilder();

         connectionBuilder.WithUrl(config.Url,
            obj => { obj.AccessTokenProvider = async () => await Task.FromResult(config.AccessToken); });
         connectionBuilder.WithAutomaticReconnect();
         var hub = connectionBuilder.Build();
         await hub.StartAsync();

         return hub;
      });
   }

   private UriBuilder GetUriBuilder(Uri uri, Dictionary<string, string> parameters)
      => new(uri)
      {
         Query = string.Join("&", parameters.Select(kvp => $"{kvp.Key}={kvp.Value}"))
      };

   private async ValueTask<AsyncError> GetError(HttpResponseMessage responseMessage, Stream content)
   {
      AsyncError error;
      if (responseMessage.StatusCode == HttpStatusCode.Unauthorized)
      {
         logger.LogError("Unauthorized request {@Uri}", responseMessage.RequestMessage?.RequestUri);
         return new AsyncError
         {
            Message = "Unauthorized request."
         };
      }

      try
      {
         error = await JsonSerializer.DeserializeAsync<AsyncError>(content, serializerOptions);
      }
      catch (Exception e)
      {
         error = new AsyncExceptionError
         {
            Message = e.Message,
            InnerException = e.InnerException?.Message
         };
      }

      logger.LogError("{@Error} {@Message} for {@Uri}",
         responseMessage.StatusCode,
         error?.Message,
         responseMessage?.RequestMessage?.RequestUri);

      return error;
   }

   public async Task<(T Result, AsyncError Exception)> GetAsync<T>(Uri uri, Dictionary<string, string> parameters)
   {
      var builder = GetUriBuilder(uri, parameters);
      var fullUri = builder.ToString();
      logger.LogDebug("{@ObjectType} Get REST call @{RestUrl}", typeof(T).Name, fullUri);
      try
      {
         var responseMessage = await httpClient.GetAsync(fullUri);
         logger.LogDebug("Response {@ResponseCode} for {@RestUrl}",
            responseMessage.StatusCode,
            fullUri);
         if (responseMessage.IsSuccessStatusCode)
         {
            try
            {
               var content = await responseMessage.Content.ReadFromJsonAsync<T>();
               logger.LogDebug("Object of type {@ObjectType} parsed for {@RestUrl}",
                  typeof(T).Name,
                  fullUri);
               return (content, null);
            }
            catch (Exception e)
            {
               logger.LogError("Error {@ErrorMessage} for when parsing ${ObjectType} for {@RestUrl}",
                  e.Message,
                  typeof(T).Name,
                  fullUri);
               return (default, new AsyncExceptionError
               {
                  InnerException = e.InnerException?.Message,
                  Message = e.Message
               });
            }
         }

         logger.LogDebug("Returning error for @{RestUrl}", fullUri);
         return (default, await GetError(responseMessage, await responseMessage.Content.ReadAsStreamAsync()));
      }
      catch (Exception e)
      {
         logger.LogError("Error {@ErrorMessage} for REST call ${ResUrl}", e.Message, fullUri);
         // The service might not be happy with us, we might have connection issues etc...
         return (default, new AsyncExceptionError
         {
            InnerException = e.InnerException?.Message,
            Message = e.Message
         });
      }
   }

   public async Task<(T Result, AsyncError Exception)> PostAsync<T>(Uri uri, object parameter)
   {
      logger.LogDebug("{@ObjectType} Post REST call @{RestUrl}", typeof(T).Name, uri);
      try
      {
         var responseMessage = await httpClient.PostAsJsonAsync(uri, parameter, serializerOptions);
         logger.LogDebug("Response {@ResponseCode} for {@RestUrl}", responseMessage.StatusCode, uri);
         await using var content = await responseMessage.Content.ReadAsStreamAsync();
         if (responseMessage.IsSuccessStatusCode)
         {
            if (string.IsNullOrEmpty(await responseMessage.Content.ReadAsStringAsync()))
            {
               return (default, null);
            }

            try
            {
               logger.LogDebug("Parse {@ObjectType} SUCCESS for { @RestUrl}", typeof(T).Name, uri);
               var result = await responseMessage.Content.ReadFromJsonAsync<T>();
               logger.LogDebug("Object of type {@ObjectType} parsed for {@RestUrl}", typeof(T).Name, uri);
               return (result, null);
            }
            catch (Exception e)
            {
               logger.LogError("Error {@ErrorMessage} for when parsing ${ObjectType} for {@RestUrl}",
                  e.Message,
                  typeof(T).Name,
                  uri);
               return (default, new AsyncExceptionError
               {
                  InnerException = e.InnerException?.Message,
                  Message = e.Message
               });
            }
         }

         logger.LogDebug("Returning error for @{RestUrl}", uri);
         return (default, await GetError(responseMessage, content));
      }
      catch (Exception e)
      {
         logger.LogError("Error {@ErrorMessage} for REST call ${ResUrl}", e.Message, uri);
         // The service might not be happy with us, we might have connection issues etc..
         return (default, new AsyncExceptionError
         {
            InnerException = e.InnerException?.Message,
            Message = e.Message
         });
      }
   }
}