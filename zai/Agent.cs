using System.Net;
using System.Text;
using System.Text.Json;
using zai.Models;

namespace zai;

public class Agent
{
    public AgentCard Card { get; }
    private readonly HttpListener _listener = new();

    public Agent(string name, string description, string version, List<Capability> capabilities, string baseUrl)
    {
        Card = new AgentCard
        {
            Name = name,
            Description = description,
            Version = version,
            Capabilities = capabilities,
            Endpoint = $"{baseUrl}/mcp/agent-card"
        };

        _listener.Prefixes.Add($"{baseUrl}/mcp/");
    }

    public void Start()
    {
        _listener.Start();
        Console.WriteLine($"Agent '{Card.Name}' listening at {Card.Endpoint}");

        _ = Task.Run(async () =>
        {
            while (true)
            {
                var ctx = await _listener.GetContextAsync();

                if (ctx.Request.Url!.AbsolutePath == "/mcp/agent-card")
                {
                    var json = JsonSerializer.Serialize(Card, new JsonSerializerOptions
                    {
                        WriteIndented = true
                    });

                    var buffer = Encoding.UTF8.GetBytes(json);
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
    }
}
