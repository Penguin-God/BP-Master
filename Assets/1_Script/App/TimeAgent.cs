using System;
using System.Collections;
using UnityEngine;

public class TimeAgent : MonoBehaviour
{
    private DraftActionController agentManager;

    public void SetInfo(DraftActionController agentManager)
    {
        this.agentManager = agentManager;
    }

    public void RequestAction(GamePhase phase, Team turn)
    {
        StartCoroutine(Co_Delay(turn));
    }

    IEnumerator Co_Delay(Team turn)
    {
        yield return null;
        agentManager.Ban(turn, 1);
    }
}
