using System.Text.Json;

namespace zai;

public class Orchestrator
{
    private readonly HttpClient _httpClient = new();

    public async Task<List<string>> FetchRegistryAsync(string registryUrl)
    {
        var json = await _httpClient.GetStringAsync(registryUrl);
        var result = JsonSerializer.Deserialize<RegistryResponse>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return result?.Agents ?? new List<string>();
    }

    public async Task<string> FetchAgentCardAsync(string endpoint)
    {
        return await _httpClient.GetStringAsync(endpoint);
    }
}

public class RegistryResponse
{
    public List<string> Agents { get; set; } = new();
}
