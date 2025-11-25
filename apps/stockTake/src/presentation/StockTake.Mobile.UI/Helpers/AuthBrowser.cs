using Duende.IdentityModel.OidcClient.Browser;

namespace StockTake.Mobile.UI.Helpers;

public class AuthBrowser : Duende.IdentityModel.OidcClient.Browser.IBrowser
{
   public async Task<BrowserResult> InvokeAsync(BrowserOptions options, CancellationToken cancellationToken = default)
   {
#if WINDOWS
        var authResult =
 await /*WinUIEx.*/
    WebAuthenticator.AuthenticateAsync(new Uri(options.StartUrl), new Uri(Constants.RedirectUri));

        string code = authResult?.Properties["code"];
        string scope = authResult?.Properties["scope"];
        string state = authResult?.Properties["state"];
        string sessionState = authResult?.Properties["session_state"];
        string response =
 $"{Constants.RedirectUri}#code={code}&scope={scope}&state={state}&session_state={sessionState}";

        return new BrowserResult()
        {
            Response = response
        };
#else
      WebAuthenticatorResult authResult =
         await WebAuthenticator.AuthenticateAsync(new Uri(options.StartUrl), new Uri(Constants.RedirectUri));

      return new BrowserResult() { Response = ParseAuthenticationResult(authResult) };
#endif
   }

   private string ParseAuthenticationResult(WebAuthenticatorResult result)
   {
      string code = result?.Properties["code"];
      string scope = result?.Properties["scope"];
      string state = result?.Properties["state"];
      string sessionState = result?.Properties["session_state"];
      return $"{Constants.RedirectUri}#code={code}&scope={scope}&state={state}&session_state={sessionState}";
   }
}