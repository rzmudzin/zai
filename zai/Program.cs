using zai;
using zai.Models;

public class Program
{
    public static async Task Main(string[] args)
    {
        // Start the registry
        var registry = new AgentRegistry("http://localhost:6000");
        registry.Start();

        // Define capabilities
        var scopeCapabilities = new List<Capability>
        {
            new Capability
            {
                Name = "AnalyzeImage",
                Description = "Accepts an image and returns bounding boxes.",
                InputSchema = "{ ... }",
                OutputSchema = "{ ... }"
            }
        };

        var floorPlanCapabilities = new List<Capability>
        {
            new Capability
            {
                Name = "GenerateFloorPlan",
                Description = "Creates a floor plan from images.",
                InputSchema = "{ ... }",
                OutputSchema = "{ ... }"
            }
        };

        // Create agents
        var scopeAgent = new Agent(
            name: "ScopeAgent",
            description: "Analyzes sensor streams.",
            version: "1.0.0",
            capabilities: scopeCapabilities,
            baseUrl: "http://localhost:5005"
        );

        var floorPlanAgent = new Agent(
            name: "FloorPlanAgent",
            description: "Generates floor plans.",
            version: "1.0.0",
            capabilities: floorPlanCapabilities,
            baseUrl: "http://localhost:5006"
        );

        // Start agents and register them
        scopeAgent.Start(registry);
        floorPlanAgent.Start(registry);

        await Task.Delay(300);

        // Orchestrator now discovers agents via registry
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
