using System;
using System.Text.Json;

namespace zai.Capabilities.Invocation
{
    public enum CapabilityInvocationStatus
    {
        Succeeded,
        Failed,
        Cancelled,
        InProgress // for streaming/progress updates
    }
}

