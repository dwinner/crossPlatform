using System.Net.Http.Headers;

namespace MauiStockTake.Client.Authentication;
public class AuthHandler : DelegatingHandler
{
    public static string AuthToken { get; set; }

    public const string AUTHENTICATED_CLIENT = "AuthenticatedClient";

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AuthToken);

        return await base.SendAsync(request, cancellationToken);
    }
}
