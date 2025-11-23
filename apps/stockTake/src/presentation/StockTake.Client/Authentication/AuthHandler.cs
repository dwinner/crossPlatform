using System.Net.Http.Headers;

namespace StockTake.Client.Authentication;

public class AuthHandler : DelegatingHandler
{
   public const string AUTHENTICATED_CLIENT = "AuthenticatedClient";
   public static string AuthToken { get; set; }

   protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
      CancellationToken cancellationToken)
   {
      request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AuthToken);

      return await base.SendAsync(request, cancellationToken);
   }
}