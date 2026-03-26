using System;
using zai.Capabilities.Invocation;

namespace zai.Agents.RunTime
{
    public sealed class InProcessAgentRuntime : IAgentRuntime
    {
        private readonly ICapabilityHandler _handler;

        public InProcessAgentRuntime(ICapabilityHandler handler)
        {
            _handler = handler;
        }

        public Task<CapabilityInvocationResponse> InvokeAsync(
            CapabilityInvocationRequest request,
            CancellationToken cancellationToken)
        {
            return _handler.InvokeAsync(
                request.CapabilityId,
                request.Payload,
                cancellationToken);
        }
    }

}

