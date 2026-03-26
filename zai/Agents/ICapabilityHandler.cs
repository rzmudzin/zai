using System;
using System.Text.Json;
using zai.Capabilities.Invocation;

namespace zai.Agents
{
    public interface ICapabilityHandler
    {
        Task<CapabilityInvocationResponse> InvokeAsync(
            string capabilityId,
            JsonElement payload,
            CancellationToken cancellationToken);
    }
}

