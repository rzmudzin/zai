namespace zai;

public class AgentRegistry
{
    private readonly List<string> _agentCardEndpoints = new();

    public void RegisterAgent(string agentCardEndpoint)
    {
        _agentCardEndpoints.Add(agentCardEndpoint);
        Console.WriteLine($"Registered agent at: {agentCardEndpoint}");
    }

    public IReadOnlyList<string> GetAllAgentEndpoints()
    {
        return _agentCardEndpoints;
    }
}
