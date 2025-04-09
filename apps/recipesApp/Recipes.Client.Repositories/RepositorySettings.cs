namespace Recipes.Client.Repositories;

public class RepositorySettings
{
   public RepositorySettings(HttpClient httpClient) => HttpClient = httpClient;

   public HttpClient HttpClient { get; }
}