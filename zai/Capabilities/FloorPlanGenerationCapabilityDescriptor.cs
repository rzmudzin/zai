using System;
using zai.Models;

namespace zai.Capabilities
{
    public sealed class FloorPlanGenerationCapabilityDescriptor : CapabilityDescriptor
    {
        public IReadOnlyList<SupportedInputFormat> InputFormats { get; init; } // e.g. "image/jpeg", "video/mp4"
        public IReadOnlyList<OutputFormat> OutputFormats { get; init; }        // e.g. "application/json+floorplan"
        public Resolution? PreferredResolution { get; init; }                  // ties into StreamProfiles
    }

}

