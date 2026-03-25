using System;
namespace zai.Capabilities
{
    public sealed class CapabilitySelectionCriteria
    {
        public IReadOnlyList<string>? RequiredTags { get; init; }
        public IReadOnlyDictionary<string, string>? Constraints { get; init; }
    }

}

