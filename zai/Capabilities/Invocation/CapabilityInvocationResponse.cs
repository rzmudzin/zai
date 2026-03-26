using System;
using System.Text.Json;
using zai.Models;

namespace zai.Capabilities.Invocation
{
    public sealed class CapabilityInvocationResponse
    {
        /// <summary>
        /// The invocation this response corresponds to.
        /// </summary>
        public Guid InvocationId { get; init; }

        /// <summary>
        /// Indicates whether the capability executed successfully.
        /// </summary>
        public bool Success { get; init; }

        /// <summary>
        /// The result payload, if successful.
        /// </summary>
        public JsonElement? Payload { get; init; }

        /// <summary>
        /// Error information, if the invocation failed.
        /// </summary>
        public ErrorInfo? Error { get; init; }
    }


}

