using System.Net.Http.Headers;
using e_trade_api.application;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace e_trade_api.Persistence;

public class CloudflareService : ICloudflareService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public CloudflareService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<bool> PurgeEverythingCache()
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(
                $"https://api.cloudflare.com/client/v4/zones/{_configuration["CloudflareClientZoneId"]}/purge_cache"
            ),
            Headers =
            {
                { "X-Auth-Email", _configuration["CloudflareMail"] },
                { "X-Auth-Key", _configuration["CloudflareKey"] }
            },
            Content = new StringContent("{\n \"purge_everything\": true\n}")
            {
                Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
            }
        };
        var response = await _httpClient.SendAsync(request);

        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        Console.WriteLine(body);

        var responseObject = JsonConvert.DeserializeObject<PurgeEverythingCacheResponse>(body);

        return responseObject.Success;
    }
}

public class PurgeEverythingCacheResponse
{
    public bool Success { get; set; }

    // public List<object> Errors { get; set; }
    // public List<object> Messages { get; set; }
    // public Result Result { get; set; }
}
