using System;
using zai.Models;

namespace zai.Capabilities
{
    public sealed class ObjectDetectionCapabilityDescriptor : CapabilityDescriptor
    {
        public IReadOnlyList<string> SupportedClasses { get; init; } // "door", "window", "wall"
        public ConfidenceRange ConfidenceRange { get; init; }
    }

}

