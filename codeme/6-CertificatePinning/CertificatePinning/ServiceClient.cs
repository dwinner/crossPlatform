using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace CertificatePinning
{
    public class ServiceClient
    {
        private HttpClient Client { get; set; }

        private HttpClientHandler clientHandler;

        public ServiceClient() 
        {
            clientHandler = new HttpClientHandler();
            clientHandler.SslProtocols =
                System.Security.Authentication.
                SslProtocols.Tls12;
            clientHandler.ServerCertificateCustomValidationCallback =
                ValidateServerCertificate;
            Client = new HttpClient(clientHandler);
        }

        public async Task MakeCallAsync(string url)
        {
            HttpResponseMessage response;
            try
            {
                // For editorial purposes only:
                response = await Client.GetAsync(url);
            }
            catch (HttpRequestException ex)
            {
                if (ex.InnerException is WebException e && e.Status ==
                    WebExceptionStatus.TrustFailure)
                {
                    response = new
                        HttpResponseMessage(
                        HttpStatusCode.MethodNotAllowed);
                }
                else
                    response = new
                        HttpResponseMessage(
                        HttpStatusCode.ServiceUnavailable);
            }
            catch (Exception ex)
            {
                response = new
                    HttpResponseMessage(
                    HttpStatusCode.InternalServerError);
            }
        }

        private bool ValidateServerCertificate(object sender,
            X509Certificate certificate, X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {
            string currentCert = certificate?.GetPublicKeyString().ToUpper();
            return EndpointConfiguration.PUBKEY.ToUpper() == currentCert;
        }
    }
}
