using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class WaitAndSelectTests
{
    [UnityTest]
    public IEnumerator 대기_후_선택()
    {
        int count = 0;
        var storage = new GameBanPickStorage(new int[] { 1 });
        AgentManager agentManager = new AgentManager(storage);
        agentManager.OnActionDone += () => count++;
        TimeAgent sut = new TimeAgent(agentManager);

        sut.RequestAction(GamePhase.Ban, Team.Red);

        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(1, storage.GetStorage(Team.Red, SelectType.Ban)[0]);
        Assert.AreEqual(1, count);
    }
}
