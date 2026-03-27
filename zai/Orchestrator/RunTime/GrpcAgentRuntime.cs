using System;
using System.Text.Json;
using Google.Protobuf;
using Grpc.Net.Client;
using zai.Agents.RunTime;

public sealed class GrpcAgentRuntime : zai.Agents.RunTime.IAgentRuntime
{
    private readonly A2A.Transport.Grpc.V1.AgentRuntime.AgentRuntimeClient _client;

    public GrpcAgentRuntime(string address)
    {
        var channel = GrpcChannel.ForAddress(address);
        _client = new A2A.Transport.Grpc.V1.AgentRuntime.AgentRuntimeClient(channel);
    }


    public async Task<zai.Capabilities.Invocation.CapabilityInvocationResponse> InvokeAsync(
            zai.Capabilities.Invocation.CapabilityInvocationRequest request,
            CancellationToken cancellationToken)
    {
        // Map domain → transport
        var grpcRequest = new A2A.Transport.Grpc.V1.CapabilityInvocationRequest
        {
            CapabilityId = request.CapabilityId,
            Payload = ByteString.CopyFromUtf8(request.Payload.GetRawText())
        };

        // Invoke agent
        A2A.Transport.Grpc.V1.CapabilityInvocationResponse grpcResponse =
            await _client.InvokeAsync(grpcRequest, cancellationToken: cancellationToken);

        // Map payload
        JsonElement? resultPayload =
            grpcResponse.Payload.Length > 0
                ? JsonDocument.Parse(grpcResponse.Payload.ToStringUtf8()).RootElement
                : (JsonElement?)null;

        // Map error
        zai.Models.ErrorInfo? error =
            grpcResponse.Error is null
                ? null
                : new zai.Models.ErrorInfo(
                    grpcResponse.Error.Code,
                    grpcResponse.Error.Message
                );

        // Map transport → domain
        var invocationIdGuid = Guid.Empty;
        Guid.TryParse(grpcResponse.InvocationId, out invocationIdGuid);
        return new zai.Capabilities.Invocation.CapabilityInvocationResponse
        {
            InvocationId = invocationIdGuid,
            Success = grpcResponse.Success,
            Payload = resultPayload,
            Error = error
        };
    }

}



