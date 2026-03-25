using zai.Capabilities;
using zai.Models;
using zai.Profiles;

namespace zai;

public class FloorPlanAgent : Agent
{
    public FloorPlanAgent(string baseUrl) : base(baseUrl) { }
    public override string AgentId => "floorplan-agent";
    public override string Name => "FloorPlanAgent";
    public override string Description => "Generates floor plans.";
    public override string Version => "1.0.0";

    public override IReadOnlyList<CapabilityDescriptor> Capabilities => new CapabilityDescriptor[]
    {
        new FloorPlanGenerationCapabilityDescriptor
        {
            CapabilityId = "generate-floorplan",
            Name = "GenerateFloorPlan",
            Version = "1.0",
            Description = "Generates a floor plan from a video stream.",
            Kind = CapabilityKind.ServerStreaming,

            InputFormats = new[]
            {
                new SupportedInputFormat("video/mp4"),
                new SupportedInputFormat("video/h264")
            },

            OutputFormats = new[]
            {
                new OutputFormat(
                    mimeType: "application/json+floorplan",
                    fileExtension: "json",
                    schema: "floorplan-v1"
                )
            },

            PreferredResolution = new PreferredResolution(
                new Resolution(1280, 720),
                weight: 1.0
            )
        }
    };



    public override List<StreamProfileBase> StreamProfiles => new List<StreamProfileBase>
    {
        new VideoProfile
        {
            Required = true,
            Resolution = "1080p",
            Fps = 30
        }
        //new VideoProfile
        //{
        //    Required = true,
        //    Resolution = "4k",
        //    Fps = 1
        //}
    };
}
