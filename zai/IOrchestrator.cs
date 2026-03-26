using System;
using zai.Capabilities.Invocation;

namespace zai
{
    public interface IOrchestrator
    {
        Task<CapabilityInvocationResponse> InvokeAsync(
            CapabilityInvocationRequest request,
            CancellationToken cancellationToken = default);
    }

}

