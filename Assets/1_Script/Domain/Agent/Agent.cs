using System;
using UnityEngine;

public class Agent
{
    private ActionAgent agent;

    public Agent(ActionAgent agent)
    {
        this.agent = agent;
    }

    public void ActoinRequset(GamePhase phase)
    {
        agent.Ban(0);
    }
}
