using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

public class MatchTests
{
    [Test]
    public void 매치_자동_진행()
    {
        PhaseData[] phase = new PhaseData[]
        {
            new PhaseData(GamePhase.Ban, new Phase(new Team[] { Team.Blue, Team.Red })),
            new PhaseData(GamePhase.Pick, new Phase(new Team[] { Team.Blue, Team.Red })),
            new PhaseData(GamePhase.Swap, new Phase(new Team[] { Team.All })),
            new PhaseData(GamePhase.Active, new Phase(new Team[] { Team.Blue, Team.Red })),
        };
        GameFlowData data = default;
        PhaseManager phaseManager = new(phase);
        phaseManager.OnFlowChanged += (flow) => data = flow;
        IActionHandler blue = new FakeActor(phaseManager);
        IActionHandler red = new FakeActor(phaseManager);

        PhaseActionDispatcher sut = new(blue, red);
        phaseManager.OnFlowChanged += sut.OnRequestAction;
        phaseManager.Start();

        Assert.AreEqual(GamePhase.Done, data.Phase);
    }

    [Test]
    public void Blue턴_Ban이면_Blue핸들러만_Ban호출()
    {
        var (blue, red, sut) = CreateArrange();

        sut.OnRequestAction(CreateData(GamePhase.Ban, Team.Blue));

        Assert.AreEqual(1, blue.Calls.Count);
        Assert.AreEqual(("Ban", Team.Blue), blue.Calls[0]);

        Assert.AreEqual(0, red.Calls.Count);
    }

    [Test]
    public void Red턴_Pick이면_Red핸들러만_Pick호출()
    {
        var (blue, red, sut) = CreateArrange();

        sut.OnRequestAction(CreateData(GamePhase.Pick, Team.Red));

        Assert.AreEqual(0, blue.Calls.Count);

        Assert.AreEqual(1, red.Calls.Count);
        Assert.AreEqual(("Pick", Team.Red), red.Calls[0]);
    }

    [Test]
    public void All턴이면_모든_팀_호출()
    {
        var (blue, red, sut) = CreateArrange();

        sut.OnRequestAction(CreateData(GamePhase.Swap, Team.All));

        Assert.AreEqual(1, blue.Calls.Count);
        Assert.AreEqual(("Swap", Team.Blue), blue.Calls[0]);

        Assert.AreEqual(1, red.Calls.Count);
        Assert.AreEqual(("Swap", Team.Red), red.Calls[0]);
    }

    [Test]
    public void Done페이즈는_어떤_핸들러도_호출하지_않는다()
    {
        var (blue, red, sut) = CreateArrange();

        sut.OnRequestAction(CreateData(GamePhase.Done, Team.All));

        Assert.AreEqual(0, blue.Calls.Count);
        Assert.AreEqual(0, red.Calls.Count);
    }

    public static GameFlowData CreateData(GamePhase phase, Team turn) => new GameFlowData(phase, turn);

    (FakeActionHandler, FakeActionHandler, PhaseActionDispatcher) CreateArrange()
    {
        var blue = new FakeActionHandler();
        var red = new FakeActionHandler();
        var sut = new PhaseActionDispatcher(blue, red);
        return (blue, red, sut);
    }
}

public class FakeActionHandler : IActionHandler
{
    public readonly List<(string Method, Team Team)> Calls = new();

    public void OnRequestBan(Team turnTeam) => Calls.Add(("Ban", turnTeam));
    public void OnRequestPick(Team turnTeam) => Calls.Add(("Pick", turnTeam));
    public void OnRequestSwap(Team turnTeam) => Calls.Add(("Swap", turnTeam));
    public void OnRequestActive(Team turnTeam) => Calls.Add(("Active", turnTeam));
}

public class FakeActor : IActionHandler
{
    readonly PhaseManager phaseManager;
    public FakeActor(PhaseManager pm) => phaseManager = pm;

    public void OnRequestBan(Team team) => phaseManager.SubmitAction(team);
    public void OnRequestPick(Team team) => phaseManager.SubmitAction(team);
    public void OnRequestSwap(Team team) => phaseManager.SubmitAction(team);
    public void OnRequestActive(Team team) => phaseManager.SubmitAction(team);
}