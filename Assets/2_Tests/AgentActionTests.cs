using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

public class AgentActionTests
{
    [Test]
    public void 현재_턴_아닌_명령_무시()
    {
        bool isDone = false;
        var storage = new GameBanPickStorage(new int[] { 1, 2, 3 });
        AgentManager sut = new(storage);
        sut.OnActionDone += () => isDone = true;

        sut.ChangePhase(GamePhase.Ban);
        sut.Pick(Team.Blue, 1);
        Assert.IsFalse(isDone);

        sut.ChangePhase(GamePhase.Pick);
        sut.Ban(Team.Blue, 1);
        Assert.IsFalse(isDone);
    }

    [Test]
    public void 픽_대행_후_알림()
    {
        bool isDone = false;
        var storage = new GameBanPickStorage(new int[] { 1, 2, 3 });
        AgentManager sut = new(storage);
        sut.OnActionDone += () => isDone = true;
        sut.ChangePhase(GamePhase.Pick);

        sut.Pick(Team.Blue, 1);

        Assert.AreEqual(1, storage.GetStorage(Team.Blue, SelectType.Pick).Count);
        Assert.AreEqual(1, storage.GetStorage(Team.Blue, SelectType.Pick)[0]);
        Assert.IsTrue(isDone);
    }

    [Test]
    public void 밴_대행_후_알림()
    {
        bool isDone = false;
        var storage = new GameBanPickStorage(new int[] { 1, 2, 3 });
        AgentManager sut = new(storage);
        sut.OnActionDone += () => isDone = true;
        sut.ChangePhase(GamePhase.Ban);

        sut.Ban(Team.Red, 1);

        Assert.AreEqual(1, storage.GetStorage(Team.Red, SelectType.Ban).Count);
        Assert.AreEqual(1, storage.GetStorage(Team.Red, SelectType.Ban)[0]);
        Assert.IsTrue(isDone);
    }

    [Test]
    public void 스왑은_요청을_각_팀에_받아야_알림()
    {
        bool isDone = false;
        AgentManager sut = new(new GameBanPickStorage(new int[] { 1, 2, 3 }));
        sut.OnActionDone += () => isDone = true;
        sut.ChangePhase(GamePhase.Swap);

        sut.SwapDone(Team.Red);
        Assert.IsFalse(isDone);
        sut.SwapDone(Team.Blue);
        Assert.IsTrue(isDone);
    }
}
