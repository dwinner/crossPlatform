using Duende.IdentityModel.OidcClient;
using StockTake.Client.Authentication;
using IBrowser = Duende.IdentityModel.OidcClient.Browser.IBrowser;

namespace StockTake.Mobile.UI.Services;

public class AuthService : IAuthService
{
   private readonly OidcClientOptions _options;

   public AuthService(IBrowser browser)
   {
      _options = new OidcClientOptions
      {
         Authority = Constants.AuthorityUri,
         ClientId = Constants.ClientId,
         Scope = Constants.Scope,
         RedirectUri = Constants.RedirectUri,
         Browser = browser
      };
   }

   public async Task<bool> LoginAsync()
   {
      var oidcClient = new OidcClient(_options);

      var loginResult = await oidcClient.LoginAsync(new LoginRequest());

      if (loginResult.IsError)
      {
         // TODO: inspect and handle error
         return false;
      }

      AuthHandler.AuthToken = loginResult.AccessToken;

      return true;
   }
}