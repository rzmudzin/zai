namespace zai;

public class Orchestrator
{
    private readonly HttpClient _httpClient = new();

    public async Task<string> FetchAgentCardAsync(string endpoint)
    {
        return await _httpClient.GetStringAsync(endpoint);
    }
}
