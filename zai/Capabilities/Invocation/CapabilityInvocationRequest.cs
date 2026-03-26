using System;
using System.Text.Json;

namespace zai.Capabilities.Invocation
{
    public sealed class CapabilityInvocationRequest
    {
        /// <summary>
        /// The agent initiating the invocation.
        /// </summary>
        public string CallerAgentId { get; init; } = default!;

        /// <summary>
        /// The agent that should execute the capability.
        /// </summary>
        public string TargetAgentId { get; init; } = default!;

        /// <summary>
        /// The capability being invoked.
        /// </summary>
        public string CapabilityId { get; init; } = default!;

        /// <summary>
        /// Unique identifier for this invocation.
        /// Used for tracing, correlation, and streaming.
        /// </summary>
        public Guid InvocationId { get; init; } = Guid.NewGuid();

        /// <summary>
        /// The input payload for the capability.
        /// This is JSON because capabilities are polymorphic.
        /// </summary>
        public JsonElement Payload { get; init; }

        /// <summary>
        /// Optional metadata for tracing, debugging, or routing.
        /// </summary>
        public Dictionary<string, string>? Metadata { get; init; }
    }


}

