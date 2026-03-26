using System;
namespace zai.Capabilities.Registration
{
    public sealed class RegisteredCapability
    {
        public string AgentId { get; }
        public CapabilityDescriptor Descriptor { get; }

        public RegisteredCapability(string agentId, CapabilityDescriptor descriptor)
        {
            AgentId = agentId ?? throw new ArgumentNullException(nameof(agentId));
            Descriptor = descriptor ?? throw new ArgumentNullException(nameof(descriptor));
        }
    }

}

