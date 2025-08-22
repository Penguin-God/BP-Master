using System;
using UnityEngine;

public class TimeAgent : MonoBehaviour
{
    private AgentManager agentManager;

    public TimeAgent(AgentManager agentManager)
    {
        this.agentManager = agentManager;
    }

    public void RequestAction(GamePhase ban, Team red)
    {
        throw new NotImplementedException();
    }
}
