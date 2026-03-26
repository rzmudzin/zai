using System;
using zai.Agents.RunTime;

namespace zai.Agents
{
    public interface IAgentDirectory
    {
        bool TryGetAgent(string agentId, out IAgentRuntime runtime);
    }

}

