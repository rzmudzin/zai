using System;
using zai.Models;

namespace zai.Capabilities
{
    public sealed class ObjectDetectionCapabilityDescriptor : CapabilityDescriptor
    {
        public IReadOnlyList<SupportedInputFormat> InputFormats { get; init; }
        public IReadOnlyList<OutputFormat> OutputFormats { get; init; }
        public IReadOnlyList<string> SupportedClasses { get; init; }
        public ConfidenceRange ConfidenceRange { get; init; }
    }


}

