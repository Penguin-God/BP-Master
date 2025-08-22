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
        DraftActionController agentManager = new DraftActionController(storage);
        agentManager.OnActionDone += () => count++;
        TimeAgent sut = new GameObject().AddComponent<TimeAgent>();
        sut.SetInfo(agentManager);

        agentManager.ChangePhase(GamePhase.Ban, Team.Red);
        sut.RequestAction(GamePhase.Ban, Team.Red);

        Assert.AreEqual(0, storage.GetStorage(Team.Red, SelectType.Ban).Count);
        Assert.AreEqual(0, count);
        yield return new WaitForSeconds(0.1f);

        Assert.AreEqual(1, storage.GetStorage(Team.Red, SelectType.Ban).Count);
        Assert.AreEqual(1, storage.GetStorage(Team.Red, SelectType.Ban)[0]);
        Assert.AreEqual(1, count);
    }
}
