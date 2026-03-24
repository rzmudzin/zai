using zai.Models;

namespace zai;

public class FloorPlanAgent : Agent
{
    public FloorPlanAgent(string baseUrl) : base(baseUrl) { }

    public override string Name => "FloorPlanAgent";
    public override string Description => "Generates floor plans.";
    public override string Version => "1.0.0";

    public override List<Capability> Capabilities => new()
    {
        new Capability
        {
            Name = "GenerateFloorPlan",
            Description = "Creates a floor plan from images.",
            InputSchema = "{ ... }",
            OutputSchema = "{ ... }"
        }
    };

    public override StreamProfile StreamProfile => new StreamProfile
    {
        Video = new VideoProfile
        {
            Required = true,
            Resolution = "4k",
            Fps = 1
        }
    };
}
