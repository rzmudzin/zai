using System;
using System.Text.Json;
using zai;
using zai.Agents;
using zai.Capabilities.Invocation;
using zai.Capabilities.Registration;
using zai.Models;

namespace zai
{
    public sealed class A2AOrchestrator : IOrchestrator
    {
        private readonly ICapabilityRegistry _capabilityRegistry;
        private readonly IAgentDirectory _agentDirectory;

        public A2AOrchestrator(
            ICapabilityRegistry capabilityRegistry,
            IAgentDirectory agentDirectory)
        {
            _capabilityRegistry = capabilityRegistry
                ?? throw new ArgumentNullException(nameof(capabilityRegistry));

            _agentDirectory = agentDirectory
                ?? throw new ArgumentNullException(nameof(agentDirectory));
        }

        public async Task<CapabilityInvocationResponse> InvokeAsync(
            CapabilityInvocationRequest request,
            CancellationToken cancellationToken = default)
        {
            // 1. Validate request
            if (request is null)
            {
                return Error("invalid-request", "Request cannot be null.", Guid.Empty);
            }

            if (string.IsNullOrWhiteSpace(request.CapabilityId))
            {
                return Error("invalid-request", "CapabilityId is required.", request.InvocationId);
            }

            // 2. Resolve capability
            if (!_capabilityRegistry.TryGetByCapabilityId(request.CapabilityId, out var registered))
            {
                return Error(
                    "unknown-capability",
                    $"Capability '{request.CapabilityId}' is not registered.",
                    request.InvocationId);
            }

            var targetAgentId = registered.AgentId;

            // 3. Validate target agent exists
            if (!_agentDirectory.TryGetAgent(targetAgentId, out var agentRuntime))
            {
                return Error(
                    "unknown-agent",
                    $"Agent '{targetAgentId}' is not available.",
                    request.InvocationId);
            }

            // 4. Forward request to agent
            try
            {
                return await agentRuntime.InvokeAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                return Error(
                    "agent-error",
                    $"Agent '{targetAgentId}' failed: {ex.Message}",
                    request.InvocationId);
            }
        }

        private static CapabilityInvocationResponse Error(
            string code,
            string message,
            Guid invocationId)
        {
            return new CapabilityInvocationResponse
            {
                InvocationId = invocationId,
                Success = false,
                Error = new ErrorInfo(code, message)
            };
        }
    }


}

/*
var response = await orchestrator.InvokeAsync(new CapabilityInvocationRequest
{
    InvocationId = Guid.NewGuid(),
    CallerAgentId = "scope-agent",
    TargetAgentId = "floorplan-agent",
    CapabilityId = "generate-floorplan",
    Payload = JsonSerializer.SerializeToElement(new { videoStreamId = "scope-video" })
});
*/

/*
public sealed class Orchestrator : IOrchestrator
{
    private readonly ICapabilityRegistry _capabilityRegistry;
    private readonly IAgentDirectory _agentDirectory; // maps AgentId → IAgentRuntime

    public Orchestrator(
        ICapabilityRegistry capabilityRegistry,
        IAgentDirectory agentDirectory)
    {
        _capabilityRegistry = capabilityRegistry;
        _agentDirectory = agentDirectory;
    }

    public async Task<CapabilityInvocationResponse> InvokeAsync(
        CapabilityInvocationRequest request,
        CancellationToken cancellationToken = default)
    {
        // 1. Validate request
        if (string.IsNullOrWhiteSpace(request.CapabilityId))
        {
            return Error(request, "invalid-request", "CapabilityId is required.");
        }

        // 2. Resolve capability
        if (!_capabilityRegistry.TryGetByCapabilityId(request.CapabilityId, out var registered))
        {
            return Error(request, "unknown-capability",
                $"Capability '{request.CapabilityId}' is not registered.");
        }

        var targetAgentId = registered.AgentId;

        // 3. Validate target agent exists
        if (!_agentDirectory.TryGetAgent(targetAgentId, out var agentRuntime))
        {
            return Error(request, "unknown-agent",
                $"Agent '{targetAgentId}' is not available.");
        }

        // 4. Forward request to agent
        try
        {
            return await agentRuntime.InvokeAsync(request, cancellationToken);
        }
        catch (Exception ex)
        {
            return Error(request, "agent-error", ex.Message);
        }
    }

    private static CapabilityInvocationResponse Error(
        CapabilityInvocationRequest request,
        string code,
        string message)
    {
        return new CapabilityInvocationResponse
        {
            InvocationId = request.InvocationId,
            Success = false,
            Error = new ErrorInfo(code, message)
        };
    }
}
*/

