using System;
using System.Text.Json;
using A2A.Transport.Grpc.V1;
using Google.Protobuf;
using Grpc.Core;

namespace zai.Agents.RunTime
{

    public sealed class GrpcAgentRuntimeService : AgentRuntime.AgentRuntimeBase
    {
        private readonly ICapabilityHandler _handler;

        public GrpcAgentRuntimeService(ICapabilityHandler handler)
        {
            _handler = handler;
        }

        public override async Task<CapabilityInvocationResponse> Invoke(
            CapabilityInvocationRequest request,
            ServerCallContext context)
        {
            var result = await _handler.InvokeAsync(
                request.CapabilityId,
                JsonDocument.Parse(request.Payload.ToStringUtf8()).RootElement,
                context.CancellationToken);

            var response = new CapabilityInvocationResponse
            {
                Success = result?.Success ?? false,
                Payload = ByteString.CopyFromUtf8(
                    result?.Payload?.GetRawText() ?? "{}"
                )
            };

            if (result is not null && !result.Success && result.Error is not null)
            {
                response.Error = new ErrorInfo
                {
                    Code = result?.Error?.Code,
                    Message = result?.Error?.Message
                };
            }

            return response;
        }
    }

}

