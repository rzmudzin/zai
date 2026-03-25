using System.Text.Json.Serialization;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "kind")]
[JsonDerivedType(typeof(VideoProfile), "video")]
[JsonDerivedType(typeof(DepthProfile), "depth")]
[JsonDerivedType(typeof(LidarProfile), "lidar")]
[JsonDerivedType(typeof(AudioProfile), "audio")]
public abstract class StreamProfileBase
{
    public string Kind { get; init; } = "";
    public string Name { get; init; } = "";
    public bool Required { get; init; }
}
