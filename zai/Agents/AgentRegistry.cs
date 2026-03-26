using System.Net;
using System.Text;
using System.Text.Json;

namespace zai.Agents;

public class AgentRegistry
{
    private readonly List<string> _agentCardEndpoints = new();
    private readonly HttpListener _listener = new();

    public AgentRegistry(string baseUrl)
    {
        _listener.Prefixes.Add($"{baseUrl}/registry/");
    }

    public void RegisterAgent(string agentCardEndpoint)
    {
        _agentCardEndpoints.Add(agentCardEndpoint);
        Console.WriteLine($"Registered agent at: {agentCardEndpoint}");
    }

    public IReadOnlyList<string> GetAllAgentEndpoints() => _agentCardEndpoints;

    public void Start()
    {
        _listener.Start();
        Console.WriteLine("Agent Registry listening...");

        _ = Task.Run(async () =>
        {
            while (true)
            {
                var ctx = await _listener.GetContextAsync();

                if (ctx.Request.Url!.AbsolutePath == "/registry/agents")
                {
                    var json = JsonSerializer.Serialize(
                        new { Agents = _agentCardEndpoints },
                        new JsonSerializerOptions { WriteIndented = true }
                    );

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
