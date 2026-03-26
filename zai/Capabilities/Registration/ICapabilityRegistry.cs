using System;
namespace zai.Capabilities.Registration
{
    public interface ICapabilityRegistry
    {
        RegisteredCapability GetByCapabilityId(string capabilityId);
        bool TryGetByCapabilityId(string capabilityId, out RegisteredCapability capability);

        IReadOnlyList<RegisteredCapability> GetByAgentId(string agentId);
        IReadOnlyDictionary<string, RegisteredCapability> GetAll();
    }

}

