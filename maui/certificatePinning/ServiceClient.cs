using System.Diagnostics;
using System.Net;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace CertificatePinning;

public class ServiceClient
{
   public ServiceClient()
   {
      var clientHandler = new HttpClientHandler();
      clientHandler.SslProtocols = SslProtocols.Tls12;
      clientHandler.ServerCertificateCustomValidationCallback = ValidateServerCertificate;
      Client = new HttpClient(clientHandler);
   }

   private HttpClient Client { get; }

   public async Task MakeCallAsync(string url)
   {
      HttpResponseMessage response;
      try
      {
         // For editorial purposes only:
         response = await Client.GetAsync(url).ConfigureAwait(true);
      }
      catch (HttpRequestException ex)
      {
         response = ex.InnerException is WebException { Status: WebExceptionStatus.TrustFailure }
            ? new HttpResponseMessage(HttpStatusCode.MethodNotAllowed)
            : new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
      }
      catch (Exception)
      {
         response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
      }

      Debug.WriteLine(response.Content.ToString());
   }

   private bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
   {
      var currentCert = certificate?.GetPublicKeyString().ToUpper();
      return EndpointConfiguration.Pubkey.ToUpper() == currentCert;
   }
}