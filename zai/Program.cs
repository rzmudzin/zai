using zai;
using zai.Models;

public class Program
{
    public static async Task Main(string[] args)
    {
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

        // Create two agents on different ports
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

        // Start both agents
        scopeAgent.Start();
        floorPlanAgent.Start();

        // Give listeners time to bind
        await Task.Delay(300);

        // Bring back the Orchestrator
        var orchestrator = new Orchestrator();

        // Fetch both MCP cards
        var scopeCardJson = await orchestrator.FetchAgentCardAsync(scopeAgent.Card.Endpoint);
        var floorCardJson = await orchestrator.FetchAgentCardAsync(floorPlanAgent.Card.Endpoint);

        Console.WriteLine("\n=== ScopeAgent MCP Card ===");
        Console.WriteLine(scopeCardJson);

        Console.WriteLine("\n=== FloorPlanAgent MCP Card ===");
        Console.WriteLine(floorCardJson);

        Console.WriteLine("\nAgents running. Press ENTER to exit.");
        Console.ReadLine();
    }
}
