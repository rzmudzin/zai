using zai.Models;

public class AgentCard
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Version { get; set; }
    public List<Capability> Capabilities { get; set; }
    public string Endpoint { get; set; }

    public List<StreamProfileBase> StreamProfiles { get; init; } = new();

}

public sealed class VideoProfile : StreamProfileBase
{
    public string? Resolution { get; init; }
    public int? Fps { get; init; }
}

public sealed class DepthProfile : StreamProfileBase
{
}

public sealed class LidarProfile : StreamProfileBase
{
}

public sealed class AudioProfile : StreamProfileBase
{
}
