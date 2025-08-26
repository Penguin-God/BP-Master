using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class MatchTests
{
    [Test]
    public void 매치는_자동_진행()
    {
        PhaseData[] phase = new PhaseData[]
        {
            new PhaseData(GamePhase.Ban, new Phase(new Team[] { Team.Blue, Team.Red })),
            new PhaseData(GamePhase.Pick, new Phase(new Team[] { Team.Blue, Team.Red })),
            new PhaseData(GamePhase.Swap, new Phase(new Team[] { Team.All })),
        };
        PhaseManager phaseManager = new(phase);

        PhaseActionRequestor blue = new PhaseActionRequestor(Team.Blue, new FakeActor());
        PhaseActionRequestor red = new PhaseActionRequestor(Team.Red, new FakeActor());

        MatchManager sut = new(phaseManager, blue, red);

        sut.GameStart();
        Assert.AreEqual(GamePhase.Done, sut.CurrentPhase);
    }
}

public class FakeActor : IActionHandler
{
    public void OnRequestBan(Team team, ActionEventBus bus) => bus.ActionDone(team);
    public void OnRequestPick(Team team, ActionEventBus bus) => bus.ActionDone(team);
    public void OnRequestSwap(Team team, ActionEventBus bus) => bus.ActionDone(team);
}