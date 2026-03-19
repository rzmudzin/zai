using zai;
using zai.Models;

public class Program
{
    public static async Task Main(string[] args)
    {
        var registry = new AgentRegistry();

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

        // Start both agents and register them
        scopeAgent.Start(registry);
        floorPlanAgent.Start(registry);

        await Task.Delay(300);

        var orchestrator = new Orchestrator();

        Console.WriteLine("\n=== Discovering Agents via Registry ===");

        foreach (var endpoint in registry.GetAllAgentEndpoints())
        {
            var cardJson = await orchestrator.FetchAgentCardAsync(endpoint);
            Console.WriteLine($"\n--- MCP Card from {endpoint} ---");
            Console.WriteLine(cardJson);
        }

        Console.WriteLine("\nAgents running. Press ENTER to exit.");
        Console.ReadLine();
    }
}
