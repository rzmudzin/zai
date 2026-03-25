using zai.Capabilities;
using zai.Models;

public sealed class AgentCard
{
    public string AgentId { get; init; } = default!;
    public string Name { get; init; } = default!;
    public string? Description { get; init; }
    public string Version { get; init; } = "1.0";

    //public List<Capability> Capabilities { get; set; }
    public string Endpoint { get; set; }

    public IReadOnlyList<CapabilityDescriptor> Capabilities { get; init; }
        = Array.Empty<CapabilityDescriptor>();

    public IReadOnlyList<StreamProfileBase> StreamProfiles { get; init; }
        = Array.Empty<StreamProfileBase>();
}



