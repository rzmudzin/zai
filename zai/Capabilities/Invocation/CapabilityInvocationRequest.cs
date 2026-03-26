using System;
using System.Text.Json;

namespace zai.Capabilities.Invocation
{
    public sealed class CapabilityInvocationRequest
    {
        public string InvocationId { get; init; }          // Correlation
        public string CallerAgentId { get; init; }
        public string TargetAgentId { get; init; }         // Optional if using logical routing
        public string CapabilityName { get; init; }        // e.g. "FloorPlan.Generate"
        public string CapabilityVersion { get; init; }     // e.g. "1.0"
        public JsonElement Payload { get; init; }          // Typed per capability
        public IReadOnlyDictionary<string, string> Metadata { get; init; }
    }

}

