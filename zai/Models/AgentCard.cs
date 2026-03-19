namespace zai.Models;

public class AgentCard
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string Version { get; set; } = "";
    public List<Capability> Capabilities { get; set; } = new();
    public string Endpoint { get; set; } = "";
}
