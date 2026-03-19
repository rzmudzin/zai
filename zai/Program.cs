using zai;

public class Program
{
    public static async Task Main(string[] args)
    {
        var registry = new AgentRegistry("http://localhost:6000");
        registry.Start();

        var scopeAgent = new ScopeAgent("http://localhost:5005");
        var floorAgent = new FloorPlanAgent("http://localhost:5006");

        scopeAgent.Start(registry);
        floorAgent.Start(registry);

        await Task.Delay(500);

        var orchestrator = new Orchestrator();

        Console.WriteLine("\n=== Discovering Agents via Registry ===");

        var agentEndpoints = await orchestrator.FetchRegistryAsync("http://localhost:6000/registry/agents");

        foreach (var endpoint in agentEndpoints)
        {
            var cardJson = await orchestrator.FetchAgentCardAsync(endpoint);
            Console.WriteLine($"\n--- MCP Card from {endpoint} ---");
            Console.WriteLine(cardJson);
        }

        Console.WriteLine("\nSystem running. Press ENTER to exit.");
        Console.ReadLine();
    }
}
