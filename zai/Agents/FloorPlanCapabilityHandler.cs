using System;
using System.Text.Json;
using zai.Capabilities.Invocation;
using zai.Models;

namespace zai.Agents
{
    public sealed class FloorPlanCapabilityHandler : ICapabilityHandler
    {
        public Task<CapabilityInvocationResponse> InvokeAsync(
            string capabilityId,
            JsonElement payload,
            CancellationToken cancellationToken)
        {
            if (capabilityId == "generate-floorplan")
            {
                // Execute capability logic here
                return Task.FromResult(new CapabilityInvocationResponse
                {
                    InvocationId = Guid.NewGuid(),
                    Success = true,
                    Payload = JsonSerializer.SerializeToElement(new
                    {
                        floorPlanUrl = "https://example.com/floorplan.json"
                    })
                });
            }

            return Task.FromResult(new CapabilityInvocationResponse
            {
                InvocationId = Guid.NewGuid(),
                Success = false,
                Error = new ErrorInfo("unknown-capability", capabilityId)
            });
        }
    }

}

