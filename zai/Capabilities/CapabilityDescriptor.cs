using System;
namespace zai.Capabilities
{
    public enum CapabilityKind
    {
        Unary,          // Request -> Response
        ServerStreaming,// Request -> Stream<Response>
        ClientStreaming,// Stream<Request> -> Response
        DuplexStreaming // Stream<Request> <-> Stream<Response>
    }

    public abstract class CapabilityDescriptor
    {
        public string CapabilityId { get; init; }          // Unique within agent
        public string Name { get; init; }                  // Logical name, e.g. "FloorPlan.Generate"
        public string Version { get; init; }               // SemVer or "1", "1.1"
        public string? Description { get; init; }

        public IReadOnlyList<string> Tags { get; init; }   // "floorplan", "vision", "geometry"
        public CapabilityKind Kind { get; init; }          // Unary, Streaming, Subscription, etc.
    }
}

