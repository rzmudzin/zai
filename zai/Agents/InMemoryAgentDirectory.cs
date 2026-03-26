using System;
using zai.Agents.RunTime;

namespace zai.Agents
{
    public sealed class InMemoryAgentDirectory : IAgentDirectory
    {
        private readonly Dictionary<string, IAgentRuntime> _agents;

        public InMemoryAgentDirectory(Dictionary<string, IAgentRuntime> agents)
        {
            _agents = agents;
        }

        public bool TryGetAgent(string agentId, out IAgentRuntime runtime)
            => _agents.TryGetValue(agentId, out runtime!);
    }

}

