using System;
using System.Text.Json;

namespace zai.Capabilities.Invocation
{
    public sealed class CapabilityInvocationResponse
    {
        public string InvocationId { get; init; }
        public CapabilityInvocationStatus Status { get; init; }
        public JsonElement? Result { get; init; }
        public ErrorInfo? Error { get; init; }
    }

}

