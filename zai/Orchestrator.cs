using System.Text.Json;

namespace zai;

public class Orchestrator
{
    private readonly HttpClient _httpClient = new();
    private VideoStream? _videoStream;
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
        //Enusre Video stream card we've obtained has a profile that meets our criteria.
        //If it does use the profile to instantiate a new instance or add a subscriptin to an existing one
        //Our criteria for video is relatively simple... ensure it publishes ifo regarding supported FPS and resolution
        if (!profile.Required)
            return;

        if (string.IsNullOrWhiteSpace(profile.Resolution))
            throw new InvalidOperationException($"Agent '{agentName}' requires video but did not specify resolution.");

        if (profile.Fps is null or <= 0)
            throw new InvalidOperationException($"Agent '{agentName}' requires video but did not specify a valid FPS.");

        if (_videoStream != null)
        {
            if (_videoStream.Resolution != profile.Resolution ||
                _videoStream.Fps != profile.Fps)
            {
                throw new InvalidOperationException(
                    $"Video stream conflict: existing stream is {_videoStream.Resolution}@{_videoStream.Fps}fps, " +
                    $"but agent '{agentName}' requires {profile.Resolution}@{profile.Fps}fps."
                );
            }

            _videoStream.AddSubscriber(agentName);
            return;
        }

        _videoStream = new VideoStream(profile.Resolution, profile.Fps.Value);
        _videoStream.Start();
        _videoStream.AddSubscriber(agentName);
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
                cards.Add(card);
        }

        //Cycle through each card we've obtained checking each if it meets our criteria for that type of stream/service
        foreach (var card in cards)
        {
            var profile = card.StreamProfile;
            if (profile == null) continue;

            if (profile.Video?.Required == true)
                ConfigureVideo(profile.Video, card.Name);

            if (profile.Depth?.Required == true)
                ConfigureDepth(profile.Depth, card.Name);

            if (profile.Lidar?.Required == true)
                ConfigureLidar(profile.Lidar, card.Name);

            if (profile.Audio?.Required == true)
                ConfigureAudio(profile.Audio, card.Name);
        }

        return cards;
    }


}

public class RegistryResponse
{
    public List<string> Agents { get; set; } = new();
}
