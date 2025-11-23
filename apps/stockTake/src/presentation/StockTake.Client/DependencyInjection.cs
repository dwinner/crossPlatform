using Microsoft.Extensions.DependencyInjection;
using StockTake.Client.Authentication;
using StockTake.Client.Services;

namespace StockTake.Client;

public static class DependencyInjection
{
    /// <summary>
    ///    Registers API client services and makes them available to inject into your code
    /// </summary>
    /// <param name="services"></param>
    /// <param name="options">Provide an instance of ApiClientOptions with auth and API parameters</param>
    /// <returns></returns>
    public static IServiceCollection AddApiClientServices(this IServiceCollection services, ApiClientOptions options)
   {
      services.AddSingleton(options);

      services.AddSingleton<AuthHandler>();

      services.AddHttpClient(AuthHandler.AUTHENTICATED_CLIENT)
         .AddHttpMessageHandler(s => s.GetService<AuthHandler>());

      services.AddSingleton<IInventoryService, InventoryService>();
      services.AddSingleton<IProductService, ProductService>();

      return services;
   }
}