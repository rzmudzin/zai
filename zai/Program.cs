using System;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using zai;
using zai.Agents;
using zai.Agents.RunTime;
using zai.Capabilities.Invocation;
using zai.Capabilities.Registration;

public class Program
{
    public static async Task Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ListenLocalhost(5001, o =>
            {
                o.Protocols = HttpProtocols.Http2;
            });
        });

        builder.Services.AddGrpc();
        builder.Services.AddSingleton<ICapabilityHandler, ScopeAgentCapabilityHandler>();

        builder.Services.AddGrpcReflection();

        var app = builder.Build();

        app.MapGrpcReflectionService();
        app.MapGrpcService<GrpcAgentRuntimeService>();

        //var serverTask = app.RunAsync();
        var serverTask = app.StartAsync();


        // 1. Start the registry
        var registry = new AgentRegistry("http://localhost:6000");
        registry.Start();

        // 2. Start agents
        var scopeAgent = new ScopeAgent("http://localhost:5005");
        var floorAgent = new FloorPlanAgent("http://localhost:5006");

        //TEST CODE: Next 3 lines just some test code working with the capabilities registry
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
            ["scope-agent"] = new InProcessAgentRuntime(new ScopeAgentCapabilityHandler()),
            ["floorplan-agent"] = new InProcessAgentRuntime(new FloorPlanCapabilityHandler())
        });


        //TEST CODE: These lines test the 
        var runtime = new GrpcAgentRuntime("http://localhost:5001");
        var serverResponse = await runtime.InvokeAsync(
            new zai.Capabilities.Invocation.CapabilityInvocationRequest
            {
                CapabilityId = "analyze-image",
                Payload = JsonDocument.Parse("{\"imageBase64\":\"image data\"}").RootElement
            },
            CancellationToken.None
        );
        Console.WriteLine($"Success: {serverResponse.Success}");
        Console.WriteLine($"Error Code: {serverResponse.Error?.Code}");
        Console.WriteLine($"Error Message: {serverResponse.Error?.Message}");
        Console.WriteLine($"Payload: {serverResponse.Payload}");


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
