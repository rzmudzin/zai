using zai.Capabilities;
using zai.Models;
using zai.Profiles;

namespace zai;

public class ScopeAgent : Agent
{
    public ScopeAgent(string baseUrl) : base(baseUrl) { }
    public override string AgentId => "scope-agent";
    public override string Name => "ScopeAgent";
    public override string Description => "Analyzes sensor streams.";
    public override string Version => "1.0.0";

    public override IReadOnlyList<CapabilityDescriptor> Capabilities => new CapabilityDescriptor[]
    {
        new ObjectDetectionCapabilityDescriptor
        {
            CapabilityId = "analyze-image",          // stable, machine-friendly
            Name = "AnalyzeImage",                   // logical capability name
            Version = "1.0",                         // semantic version
            Description = "Accepts an image and returns bounding boxes.",
            Kind = CapabilityKind.Unary,

            InputFormats = new[]
            {
                new SupportedInputFormat("image/jpeg"),
                new SupportedInputFormat("image/png")
            },

            OutputFormats = new[]
            {
                new OutputFormat("application/json", "json", schema: "bounding-boxes-v1")
            },

            ConfidenceRange = new ConfidenceRange(0.0, 1.0)
        }
    };

    public override List<StreamProfileBase> StreamProfiles => new List<StreamProfileBase> {
        new VideoProfile
        {
            Required = true,
            Resolution = "1080p",
            Fps = 30
        },
        new DepthProfile
        {
            Required = true
        }
    };
}
