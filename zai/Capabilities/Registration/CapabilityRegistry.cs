using System;
namespace zai.Capabilities.Registration
{
    public sealed class CapabilityRegistry : ICapabilityRegistry
    {
        private readonly Dictionary<string, RegisteredCapability> _byCapabilityId = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, List<RegisteredCapability>> _byAgentId = new(StringComparer.OrdinalIgnoreCase);

        public CapabilityRegistry(IEnumerable<AgentCard> agentCards)
        {
            if (agentCards == null) throw new ArgumentNullException(nameof(agentCards));

            foreach (var card in agentCards)
            {
                if (card.AgentId is null)
                    throw new InvalidOperationException("AgentCard.AgentId cannot be null.");

                if (card.Capabilities is null)
                    continue;

                foreach (var capability in card.Capabilities)
                {
                    if (string.IsNullOrWhiteSpace(capability.CapabilityId))
                        throw new InvalidOperationException($"Capability on agent '{card.AgentId}' has no CapabilityId.");

                    var registered = new RegisteredCapability(card.AgentId, capability);

                    if (_byCapabilityId.ContainsKey(capability.CapabilityId))
                    {
                        throw new InvalidOperationException(
                            $"Duplicate CapabilityId '{capability.CapabilityId}' found on agent '{card.AgentId}'.");
                    }

                    _byCapabilityId[capability.CapabilityId] = registered;

                    if (!_byAgentId.TryGetValue(card.AgentId, out var list))
                    {
                        list = new List<RegisteredCapability>();
                        _byAgentId[card.AgentId] = list;
                    }

                    list.Add(registered);
                }
            }
        }

        public RegisteredCapability GetByCapabilityId(string capabilityId)
        {
            if (capabilityId == null) throw new ArgumentNullException(nameof(capabilityId));

            if (!_byCapabilityId.TryGetValue(capabilityId, out var capability))
            {
                throw new KeyNotFoundException($"CapabilityId '{capabilityId}' is not registered.");
            }

            return capability;
        }

        public bool TryGetByCapabilityId(string capabilityId, out RegisteredCapability capability)
        {
            if (capabilityId == null) throw new ArgumentNullException(nameof(capabilityId));
            return _byCapabilityId.TryGetValue(capabilityId, out capability!);
        }

        public IReadOnlyList<RegisteredCapability> GetByAgentId(string agentId)
        {
            if (agentId == null) throw new ArgumentNullException(nameof(agentId));
            return _byAgentId.TryGetValue(agentId, out var list)
                ? (IReadOnlyList<RegisteredCapability>)list
                : Array.Empty<RegisteredCapability>();
        }

        public IReadOnlyDictionary<string, RegisteredCapability> GetAll()
            => _byCapabilityId;
    }


}

