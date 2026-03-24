using zai.Models;

namespace zai;

public class ScopeAgent : Agent
{
    public ScopeAgent(string baseUrl) : base(baseUrl) { }

    public override string Name => "ScopeAgent";
    public override string Description => "Analyzes sensor streams.";
    public override string Version => "1.0.0";

    public override List<Capability> Capabilities => new()
    {
        new Capability
        {
            Name = "AnalyzeImage",
            Description = "Accepts an image and returns bounding boxes.",
            InputSchema = "{ ... }",
            OutputSchema = "{ ... }"
        }
    };

    public override StreamProfile StreamProfile => new StreamProfile
    {
        Video = new VideoProfile
        {
            Required = true,
            Resolution = "1080p",
            Fps = 30
        },
        Depth = new DepthProfile
        {
            Required = true
        }
    };
}
