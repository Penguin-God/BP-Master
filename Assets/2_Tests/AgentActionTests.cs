using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

public class AgentActionTests
{
    [Test]
    public void 현재_팀_아닌_명령_무시()
    {
        const int Id = 1;
        (GameBanPickStorage storage, AgentManager sut) = CreateActors(Id);
        bool isDone = false;
        sut.OnActionDone += () => isDone = true;

        sut.ChangePhase(GamePhase.Ban, Team.Red);
        sut.Ban(Team.Blue, Id);
        Assert.IsFalse(isDone);
    }

    [Test]
    public void 현재_페이즈_아닌_명령_무시()
    {
        const int Id = 1;
        (GameBanPickStorage storage, AgentManager sut) = CreateActors(Id);
        bool isDone = false;
        sut.OnActionDone += () => isDone = true;

        sut.ChangePhase(GamePhase.Ban);
        sut.Pick(Team.Blue, Id);
        Assert.IsFalse(isDone);

        sut.ChangePhase(GamePhase.Pick);
        sut.Ban(Team.Blue, Id);
        Assert.IsFalse(isDone);
    }

    [Test]
    public void 픽_대행_후_알림()
    {
        bool isDone = false;
        (GameBanPickStorage storage, AgentManager sut) = CreateActors(1);
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
        (GameBanPickStorage storage, AgentManager sut) = CreateActors(1);
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
        AgentManager sut = new(new GameBanPickStorage(new int[] { }));
        sut.OnActionDone += () => isDone = true;
        sut.ChangePhase(GamePhase.Swap);

        sut.SwapDone(Team.Red);
        Assert.IsFalse(isDone);
        sut.SwapDone(Team.Blue);
        Assert.IsTrue(isDone);
    }

    (GameBanPickStorage, AgentManager) CreateActors(int id)
    {
        var storage = new GameBanPickStorage(new int[] { id });
        AgentManager sut = new(storage);
        return (storage, sut);
    }
}
