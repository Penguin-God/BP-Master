using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ActionHandleTests
{
    [Test]
    public void 요청하면_그에_맞는_행동_실행()
    {
        GameBanPickStorage result = new GameBanPickStorage(new int[] { 1 });
        IActionHandler sut = new TestActionHandler(Team.Red, result);
        DraftActionController draftAction = new DraftActionController(result);
        int count = 0;
        draftAction.OnActionDone += () => count++;

        draftAction.ChangePhase(GamePhase.Pick, Team.Red);
        sut.OnRequestAction(draftAction, GamePhase.Pick);

        Assert.AreEqual(1, result.GetStorage(Team.Red, SelectType.Pick)[0]);
        Assert.AreEqual(1, count);
    }


    [Test]
    [TestCase(GamePhase.Ban)]
    [TestCase(GamePhase.Pick)]
    [TestCase(GamePhase.Swap)]
    [TestCase(GamePhase.Done)]
    public void 요청_페이즈에_따른_행동_실행(GamePhase phase)
    {
        var team = default(Team);
        var action = default(DraftActionController);
        var fake = new FakeExecutor();
        var sut = new PhaseActionDispatcher(team, fake);

        sut.OnRequestAction(action, phase);

        Assert.AreEqual(phase, fake.Phase);
    }
}


public class FakeExecutor : IDraftActionHandler
{
    public GamePhase Phase = GamePhase.Done;

    public void OnRequestBan(Team team, DraftActionController draftAction) => Phase = GamePhase.Ban;

    public void OnRequestPick(Team team, DraftActionController draftAction) => Phase = GamePhase.Pick;

    public void OnRequestSwap(Team team, DraftActionController draftAction) => Phase = GamePhase.Swap;
}


public class TestActionHandler : IActionHandler
{
    readonly Team Team;
    readonly GameBanPickStorage Storage;
    public TestActionHandler(Team team, GameBanPickStorage storage)
    {
        Team = team;
        Storage = storage;
    }

    public void OnRequestAction(DraftActionController draftAction, GamePhase phase)
    {
        switch (phase)
        {
            case GamePhase.Ban: draftAction.Ban(Team, Storage.SelectableIds[0]); break;
            case GamePhase.Pick: draftAction.Pick(Team, Storage.SelectableIds[0]); break;
            case GamePhase.Swap: draftAction.SwapDone(Team); break;
        }
    }

}
