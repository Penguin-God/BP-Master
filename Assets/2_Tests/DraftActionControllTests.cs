using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

public class DraftActionControllTests
{
    [TestCase(GamePhase.Ban, Team.Blue, SelectType.Ban)]
    [TestCase(GamePhase.Ban, Team.Red, SelectType.Ban)]
    [TestCase(GamePhase.Pick, Team.Blue, SelectType.Pick)]
    [TestCase(GamePhase.Pick, Team.Red, SelectType.Pick)]
    public void 올바른_페이즈와_팀이면_true와_알림(GamePhase phase, Team team, SelectType type)
    {
        bool isDone = false;
        (GameBanPickStorage storage, DraftActionController sut) = CreateActors(1);
        sut.OnActionDone += () => isDone = true;

        sut.ChangePhase(phase, team);
        bool result = (phase == GamePhase.Ban) ? sut.Ban(team, 1): sut.Pick(team, 1);

        Assert.IsTrue(result);
        Assert.AreEqual(1, storage.GetStorage(team, type).Count);
        Assert.AreEqual(1, storage.GetStorage(team, type)[0]);
        Assert.IsTrue(isDone);
    }

    [TestCase(GamePhase.Ban, Team.Blue, Team.Red)] // 턴 Red인데 Blue가 Ban
    [TestCase(GamePhase.Ban, Team.Red, Team.Blue)]
    [TestCase(GamePhase.Pick, Team.Blue, Team.Red)]
    [TestCase(GamePhase.Pick, Team.Red, Team.Blue)]
    public void 잘못된_팀이면_false와_알림없음(GamePhase phase, Team currentTurn, Team wrongTeam)
    {
        bool isDone = false;
        (GameBanPickStorage storage, DraftActionController sut) = CreateActors(1);
        sut.OnActionDone += () => isDone = true;

        sut.ChangePhase(phase, currentTurn);
        bool result = (phase == GamePhase.Ban) ? sut.Ban(wrongTeam, 1) : sut.Pick(wrongTeam, 1);

        Assert.IsFalse(result);
        Assert.AreEqual(0, storage.GetStorage(wrongTeam, SelectType.Ban).Count);
        Assert.AreEqual(0, storage.GetStorage(wrongTeam, SelectType.Pick).Count);
        Assert.IsFalse(isDone);
    }

    [Test]
    public void 스왑은_요청을_각_팀에_받아야_알림()
    {
        bool isDone = false;
        DraftActionController sut = new(new GameBanPickStorage(new int[] { }));
        sut.OnActionDone += () => isDone = true;
        sut.ChangePhase(GamePhase.Swap, Team.All);

        sut.SwapDone(Team.Red);
        Assert.IsFalse(isDone);
        sut.SwapDone(Team.Blue);
        Assert.IsTrue(isDone);
    }

    (GameBanPickStorage, DraftActionController) CreateActors(int id)
    {
        var storage = new GameBanPickStorage(new int[] { id });
        DraftActionController sut = new(storage);
        return (storage, sut);
    }
}
