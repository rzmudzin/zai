using System;
using System.Text.Json;
using zai.Capabilities.Invocation;
using zai.Models;

namespace zai.Agents
{
    public sealed class ScopeAgentCapabilityHandler : ICapabilityHandler
    {
        public async Task<CapabilityInvocationResponse> InvokeAsync(
            string capabilityId,
            JsonElement payload,
            CancellationToken cancellationToken)
        {
            try
            {
                switch (capabilityId)
                {
                    case "analyze-image":
                        return await HandleAnalyzeImageAsync(payload, cancellationToken);

                    // Add future ScopeAgent capabilities here
                    // case "some-other-capability":
                    //     return await HandleSomeOtherCapabilityAsync(payload, cancellationToken);

                    default:
                        return UnknownCapability(capabilityId);
                }
            }
            catch (Exception ex)
            {
                return Error("scope-agent-error", ex.Message);
            }
        }

        // ---------------------------------------------------------------------
        // Capability Implementations
        // ---------------------------------------------------------------------

        private Task<CapabilityInvocationResponse> HandleAnalyzeImageAsync(
            JsonElement payload,
            CancellationToken cancellationToken)
        {
            // Example: Extract input fields from payload
            // (Your real implementation will replace this placeholder logic)

            // For example, if payload contains:
            // { "imageBase64": "..." }

            if (!payload.TryGetProperty("imageBase64", out var imageProp))
            {
                return Task.FromResult(Error(
                    "invalid-payload",
                    "Missing required field: imageBase64"));
            }

            string imageBase64 = imageProp.GetString()!;

            // TODO: Replace with real image analysis logic
            var fakeBoundingBoxes = new[]
            {
            new { x = 10, y = 20, width = 100, height = 80 },
            new { x = 200, y = 150, width = 60, height = 40 }
        };

            var responsePayload = JsonSerializer.SerializeToElement(new
            {
                boundingBoxes = fakeBoundingBoxes
            });

            return Task.FromResult(new CapabilityInvocationResponse
            {
                Success = true,
                Payload = responsePayload
            });
        }

        // ---------------------------------------------------------------------
        // Helpers
        // ---------------------------------------------------------------------

        private static CapabilityInvocationResponse UnknownCapability(string capabilityId)
            => Error("unknown-capability", $"ScopeAgent does not implement '{capabilityId}'.");

        private static CapabilityInvocationResponse Error(string code, string message)
            => new CapabilityInvocationResponse
            {
                Success = false,
                Error = new ErrorInfo(code, message)
            };
    }

}

