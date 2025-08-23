using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ActionHandleTests
{
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
