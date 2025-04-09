using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PartsClient.Data;

public static class PartsManager
{
    // TODO: Add fields for BaseAddress, Url, and authorizationKey
    static readonly string BaseAddress = "URL GOES HERE";
    static readonly string Url = $"{BaseAddress}/api/";
    private static string authKey = "";

    static HttpClient client;

    private static async Task<HttpClient> GetClient()
    {
        if (client!=null)
        {
            return client;
        }

        client = new HttpClient();
        if (string.IsNullOrEmpty(authKey))
        {
            authKey = await client.GetStringAsync($"{Url}login");
            authKey = JsonSerializer.Deserialize<string>(authKey);
        }
        
        client.DefaultRequestHeaders.Add("Authorization",authKey);
        client.DefaultRequestHeaders.Add("Accept","application/json");

        return client;
    }

    public static async Task<IEnumerable<Part>> GetAll()
    {
        if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
        {
            return new List<Part>();
        }

        var httpClient = await GetClient();
        var result = await httpClient.GetStringAsync($"{Url}parts");
        var allParts = JsonSerializer.Deserialize<List<Part>>(result, new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        });

        return allParts;
    }

    public static async Task<Part> Add(string partName, string supplier, string partType)
    {
        if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
        {
            return new Part();
        }

        Part part = new Part()
        {
            PartName = partName,
            Suppliers = new List<string>(new[] { supplier }),
            PartID = string.Empty,
            PartType = partType,
            PartAvailableDate = DateTime.Now
        };

        var httpClient = await GetClient();
        HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Post, $"{Url}parts");
        msg.Content = JsonContent.Create<Part>(part);
        var responseMessage = await httpClient.SendAsync(msg);
        responseMessage.EnsureSuccessStatusCode();
        var returnedJson = await responseMessage.Content.ReadAsStringAsync();
        var insertedPart = JsonSerializer.Deserialize<Part>(returnedJson, new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        });

        return insertedPart;
    }

    public static async Task Update(Part part)
    {
        if (Connectivity.Current.NetworkAccess!=NetworkAccess.Internet)
        {
            return;
        }

        HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Put, $"{Url}parts/{part.PartID}");
        msg.Content=JsonContent.Create(part);
        var httpClient = await GetClient();
        var response = await httpClient.SendAsync(msg);
        response.EnsureSuccessStatusCode();
    }

    public static async Task Delete(string partID)
    {
        if (Connectivity.Current.NetworkAccess!=NetworkAccess.Internet)
        {
            return;
        }

        HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Delete, $"{Url}parts/{partID}");
        var httpClient = await GetClient();
        var response = await httpClient.SendAsync(msg);
        response.EnsureSuccessStatusCode();
    }
}
