using System;
using System.Text.Json.Serialization;

namespace zai.Capabilities
{
    public enum CapabilityKind
    {
        Unary,          // Request -> Response
        ServerStreaming,// Request -> Stream<Response>
        ClientStreaming,// Stream<Request> -> Response
        DuplexStreaming // Stream<Request> <-> Stream<Response>
    }

    [JsonPolymorphic(TypeDiscriminatorPropertyName = "descriptorType")]
    [JsonDerivedType(typeof(FloorPlanGenerationCapabilityDescriptor), "floorplan")]
    [JsonDerivedType(typeof(ObjectDetectionCapabilityDescriptor), "object-detection")]
    public abstract class CapabilityDescriptor
    {
        public string CapabilityId { get; init; } = default!;
        public string Name { get; init; } = default!;
        public string Version { get; init; } = default!;
        public string Description { get; init; } = default!;
        public CapabilityKind Kind { get; init; }

        // Restored property
        public IReadOnlyList<string> Tags { get; init; } = Array.Empty<string>();
    }


}

