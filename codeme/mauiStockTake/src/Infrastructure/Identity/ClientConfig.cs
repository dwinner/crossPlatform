using System;
using Duende.IdentityServer.Models;

namespace MauiStockTake.Infrastructure.Identity;

public class ClientConfig
{
    public ClientConfig()
    {
    }

    public static IEnumerable<Client> GetClients()
    {
        return new List<Client>()
        {
            new Client
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
}

