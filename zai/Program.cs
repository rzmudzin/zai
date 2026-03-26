using System;
using System.Text.Json;
using zai;
using zai.Agents;
using zai.Agents.RunTime;
using zai.Capabilities.Invocation;
using zai.Capabilities.Registration;

public class Program
{
    public static async Task Main(string[] args)
    {
        // 1. Start the registry
        var registry = new AgentRegistry("http://localhost:6000");
        registry.Start();

        // 2. Start agents
        var scopeAgent = new ScopeAgent("http://localhost:5005");
        var floorAgent = new FloorPlanAgent("http://localhost:5006");

        //Next 3 lines just some test code working with the capabilities registry
        //var agentCards = new AgentCard[] { scopeAgent.Card, floorAgent.Card };
        //ICapabilityRegistry capabilityRegistry = new CapabilityRegistry(agentCards);
        //var generateFloorPanCapability = capabilityRegistry.GetByCapabilityId("generate-floorplan");

        scopeAgent.Start(registry);
        floorAgent.Start(registry);

        // Give agents a moment to register
        await Task.Delay(300);


        // 3. Create orchestrator
        var registryUrl = "http://localhost:6000/registry/agents";
        var bootstrapOrchestrator = new Orchestrator();
        var cards = await bootstrapOrchestrator.DiscoverAgentsAndConfigureStreams(registryUrl);

        // Build the capability registry
        var capabilityRegistry = new CapabilityRegistry(cards);


        //var orchestrator = new Orchestrator();

        //Console.WriteLine("\n=== Discovering Agents and Configuring Streams ===");

        //// 4. Discover agents + configure streams based on StreamProfile
        //var cards = await orchestrator.DiscoverAgentsAndConfigureStreams(
        //    "http://localhost:6000/registry/agents"
        //);

        // 5. Print MCP cards for visibility
        foreach (var card in cards)
        {
            Console.WriteLine($"\n--- MCP Card for {card.Name} ---");
            Console.WriteLine($"Endpoint: {card.Endpoint}");
            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(
                card,
                new System.Text.Json.JsonSerializerOptions { WriteIndented = true }
            ));
        }

        var agentDirectory = new InMemoryAgentDirectory(new Dictionary<string, IAgentRuntime>
        {
            //["scope-agent"] = new InProcessAgentRuntime(new ScopeAgentCapabilityHandler()),
            ["floorplan-agent"] = new InProcessAgentRuntime(new FloorPlanCapabilityHandler())
        });
        var a2a = new A2AOrchestrator(capabilityRegistry, agentDirectory);
        var response = await a2a.InvokeAsync(new CapabilityInvocationRequest
        {
            InvocationId = Guid.NewGuid(),
            CallerAgentId = "scope-agent",
            TargetAgentId = "floorplan-agent",
            CapabilityId = "generate-floorplan",
            Payload = JsonSerializer.SerializeToElement(new
            {
                videoStreamId = "scope-video-720p"
            })
        });



        //Console.WriteLine("\nSystem initialized. Press ENTER to shut down.");
        //Console.ReadLine();

        // 6. Graceful shutdown
        //scopeAgent.Stop(registry);
        //floorAgent.Stop(registry);

        //Console.WriteLine("Shutdown requested. Cleaning up...");
        //await Task.Delay(300);

        Console.WriteLine("\nSystem initialized. Press CTRL+C to exit.");
        await Task.Delay(Timeout.Infinite);


    }
}
