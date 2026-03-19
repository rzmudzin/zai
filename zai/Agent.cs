using System.Net;
using System.Text;
using System.Text.Json;
using zai.Models;

namespace zai;

public abstract class Agent
{
    public abstract string Name { get; }
    public abstract string Description { get; }
    public abstract string Version { get; }
    public abstract List<Capability> Capabilities { get; }

    public AgentCard Card { get; private set; }

    private readonly HttpListener _listener = new();
    private readonly string _baseUrl;

    protected Agent(string baseUrl)
    {
        _baseUrl = baseUrl;

        Card = new AgentCard
        {
            Name = Name,
            Description = Description,
            Version = Version,
            Capabilities = Capabilities,
            Endpoint = $"{_baseUrl}/mcp/agent-card"
        };

        _listener.Prefixes.Add($"{_baseUrl}/mcp/");
    }

    public void Start(AgentRegistry registry)
    {
        registry.RegisterAgent(Card.Endpoint);

        _listener.Start();
        Console.WriteLine($"Agent '{Name}' listening at {Card.Endpoint}");

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
