using System.Text.Json;
using zai.Profiles;

namespace zai;

public class Orchestrator
{
    private readonly HttpClient _httpClient = new();
    private readonly Dictionary<string, VideoStream> _videoStreams = new();
    private DepthStream? _depthStream;
    private LidarStream? _lidarStream;
    private AudioStream? _audioStream;


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

    private void ConfigureAudio(AudioProfile profile, string agentName)
    {
        if (!profile.Required)
            return;

        if (_audioStream != null)
        {
            _audioStream.AddSubscriber(agentName);
            return;
        }

        _audioStream = new AudioStream();
        _audioStream.Start();
        _audioStream.AddSubscriber(agentName);
    }

    private void ConfigureLidar(LidarProfile profile, string agentName)
    {
        if (!profile.Required)
            return;

        if (_lidarStream != null)
        {
            _lidarStream.AddSubscriber(agentName);
            return;
        }

        _lidarStream = new LidarStream();
        _lidarStream.Start();
        _lidarStream.AddSubscriber(agentName);
    }

    private void ConfigureDepth(DepthProfile profile, string agentName)
    {
        if (!profile.Required)
            return;

        if (_depthStream != null)
        {
            _depthStream.AddSubscriber(agentName);
            return;
        }

        _depthStream = new DepthStream();
        _depthStream.Start();
        _depthStream.AddSubscriber(agentName);
    }

    private void ConfigureVideo(VideoProfile profile, string agentName)
    {
        if (!profile.Required)
            return;

        if (string.IsNullOrWhiteSpace(profile.Resolution))
            throw new InvalidOperationException($"Agent '{agentName}' requires video but did not specify resolution.");

        if (profile.Fps is null or <= 0)
            throw new InvalidOperationException($"Agent '{agentName}' requires video but did not specify a valid FPS.");

        // Unique key for this video profile
        var key = $"{profile.Resolution}@{profile.Fps}";

        // If a stream with this profile already exists, subscribe to it
        if (_videoStreams.TryGetValue(key, out var existingStream))
        {
            existingStream.AddSubscriber(agentName);
            return;
        }

        // Otherwise create a new stream for this profile
        var newStream = new VideoStream(profile.Resolution, profile.Fps.Value);
        newStream.Start();
        newStream.AddSubscriber(agentName);

        _videoStreams[key] = newStream;
    }


    public async Task<List<AgentCard>> DiscoverAgentsAndConfigureStreams(string registryUrl)
    {
        var endpoints = await FetchRegistryAsync(registryUrl);
        var cards = new List<AgentCard>();

        foreach (var endpoint in endpoints)
        {
            var json = await FetchAgentCardAsync(endpoint);

            var card = JsonSerializer.Deserialize<AgentCard>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            if (card != null)
            {
                cards.Add(card);
                foreach (var profile in card.StreamProfiles)
                {
                    switch (profile)
                    {
                        case VideoProfile v:
                            ConfigureVideo(v, card.Name);
                            break;

                        case DepthProfile d:
                            ConfigureDepth(d, card.Name);
                            break;

                        case LidarProfile l:
                            ConfigureLidar(l, card.Name);
                            break;

                        case AudioProfile a:
                            ConfigureAudio(a, card.Name);
                            break;
                    }
                }
            }
        }

        return cards;
    }


}

public class RegistryResponse
{
    public List<string> Agents { get; set; } = new();
}
