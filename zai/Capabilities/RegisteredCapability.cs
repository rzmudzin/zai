using System;
namespace zai.Capabilities
{
    public sealed class RegisteredCapability
    {
        public string AgentId { get; }
        public CapabilityDescriptor Descriptor { get; }

        public RegisteredCapability(string agentId, CapabilityDescriptor descriptor)
        {
            AgentId = agentId;
            Descriptor = descriptor;
        }
    }

}

