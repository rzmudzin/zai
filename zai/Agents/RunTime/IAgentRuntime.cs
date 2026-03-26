using System;
using zai.Capabilities.Invocation;

namespace zai.Agents.RunTime
{
    public interface IAgentRuntime
    {
        Task<CapabilityInvocationResponse> InvokeAsync(
            CapabilityInvocationRequest request,
            CancellationToken cancellationToken);
    }

}

