using System;
using System.Text.Json;

namespace zai.Capabilities.Invocation
{
    public sealed class CapabilityStreamMessage
    {
        public string InvocationId { get; init; }
        public int SequenceNumber { get; init; }
        public JsonElement Payload { get; init; }          // Partial result / progress
        public bool IsFinal { get; init; }
    }

}

