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
            new PhaseData(GamePhase.Active, new Phase(new Team[] { Team.Blue, Team.Red })),
        };
        PhaseManager phaseManager = new(phase);

        var bus = new ActionEventBus();
        PhaseActionRequestor blue = new PhaseActionRequestor(Team.Blue, new FakeActor(phaseManager));
        PhaseActionRequestor red = new PhaseActionRequestor(Team.Red, new FakeActor(phaseManager));

        MatchManager sut = new(phaseManager, bus, blue, red);

        sut.GameStart();
        Assert.AreEqual(GamePhase.Done, sut.CurrentPhase);
    }
}

public class FakeActor : IActionHandler
{
    readonly PhaseManager bus;
    public FakeActor(PhaseManager bus) => this.bus = bus;

    public void OnRequestBan(Team team) => bus.SubmitAction(team);
    public void OnRequestPick(Team team) => bus.SubmitAction(team);
    public void OnRequestSwap(Team team) => bus.SubmitAction(team);
    public void OnRequestActive(Team team) => bus.SubmitAction(team);
}