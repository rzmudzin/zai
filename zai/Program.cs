using System.Net;
using System.Text.Json;
using zai.Models;

namespace zai;

public class Program
{
    public static async Task Main(string[] args)
    {
        var agentCard = new AgentCard
        {
            Name = "ScopeAgent",
            Description = "Analyzes sensor streams and provides guidance.",
            Version = "1.0.0",
            Capabilities = new List<Capability>
            {
                new Capability
                {
                    Name = "AnalyzeImage",
                    Description = "Accepts an image and returns bounding boxes.",
                    InputSchema = "{ \"type\": \"object\", \"properties\": { \"image\": { \"type\": \"string\", \"format\": \"base64\" }}}",
                    OutputSchema = "{ \"type\": \"object\", \"properties\": { \"boxes\": { \"type\": \"array\" }}}"
                }
            },
            Endpoint = "http://localhost:5005/mcp/agent-card"
        };

        // Start MCP Agent Card server
        var listener = new HttpListener();
        listener.Prefixes.Add("http://localhost:5005/mcp/");
        listener.Start();

        Console.WriteLine("MCP Agent Card server running on http://localhost:5005/mcp/agent-card");

        _ = Task.Run(async () =>
        {
            while (true)
            {
                var ctx = await listener.GetContextAsync();
                if (ctx.Request.Url!.AbsolutePath == "/mcp/agent-card")
                {
                    var json = JsonSerializer.Serialize(agentCard, new JsonSerializerOptions
                    {
                        WriteIndented = true
                    });

                    var buffer = System.Text.Encoding.UTF8.GetBytes(json);
                    ctx.Response.ContentType = "application/json";
                    ctx.Response.OutputStream.Write(buffer, 0, buffer.Length);
                    ctx.Response.Close();
                }
                else
                {
                    ctx.Response.StatusCode = 404;
                    ctx.Response.Close();
                }
            }
        });

        // Use the new Orchestrator class
        var orchestrator = new Orchestrator();

        await Task.Delay(500); // Give server time to start

        var cardJson = await orchestrator.FetchAgentCardAsync(agentCard.Endpoint);

        Console.WriteLine("\nFetched MCP Agent Card:");
        Console.WriteLine(cardJson);

        Console.WriteLine("\nPress ENTER to exit.");
        Console.ReadLine();
    }
}
