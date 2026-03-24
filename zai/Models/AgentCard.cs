using zai.Models;

public class AgentCard
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Version { get; set; }
    public List<Capability> Capabilities { get; set; }
    public string Endpoint { get; set; }

    // NEW
    public StreamProfile StreamProfile { get; set; }
}

public class StreamProfile
{
    public VideoProfile? Video { get; set; }
    public DepthProfile? Depth { get; set; }
    public LidarProfile? Lidar { get; set; }
    public AudioProfile? Audio { get; set; }
}

public class VideoProfile
{
    public bool Required { get; set; }
    public string? Resolution { get; set; }
    public int? Fps { get; set; }
}

public class DepthProfile
{
    public bool Required { get; set; }
}

public class LidarProfile
{
    public bool Required { get; set; }
}

public class AudioProfile
{
    public bool Required { get; set; }
}
