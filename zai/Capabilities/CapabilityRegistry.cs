using System;
namespace zai.Capabilities
{
    public sealed class CapabilityRegistry
    {
        private readonly Dictionary<string, List<RegisteredCapability>> _byName = new();

        public void Register(AgentCard card)
        {
            foreach (var cap in card.Capabilities)
            {
                var key = cap.Name; // e.g. "FloorPlan.Generate"
                if (!_byName.TryGetValue(key, out var list))
                {
                    list = new List<RegisteredCapability>();
                    _byName[key] = list;
                }

                list.Add(new RegisteredCapability(card.AgentId, cap));
            }
        }

        public RegisteredCapability? Resolve(string name, string? version, CapabilitySelectionCriteria criteria)
        {
            // Deterministic selection logic here
            return null;
        }
    }

}

