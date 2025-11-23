using Duende.IdentityServer.Models;

namespace StockTake.Infrastructure.Identity;

public class ClientConfig
{
   public static IEnumerable<Client> GetClients() =>
      new List<Client>
      {
         new()
         {
            ClientName = "MauiStockTake.Client",
            ClientId = "com.mildredsurf.stocktake",
            AllowedGrantTypes = GrantTypes.Code,
            RequirePkce = true,
            AllowOfflineAccess = true,
            AbsoluteRefreshTokenLifetime = 2592000,
            RequireClientSecret = false,
            RedirectUris = new List<string>
            {
               "auth.com.mildredsurf.stocktake://callback"
            },
            AllowedScopes = new List<string>
            {
               "openid",
               "profile",
               "offline",
               "MauiStockTake.WebAPIAPI"
            },
            AlwaysIncludeUserClaimsInIdToken = true
         }
      };
}