using NUnit.Framework;
using System.Collections.Generic;

public class PhaseEventRaiseTests
{
    PhaseEventDispatcher CreateSut() => new PhaseEventDispatcher();

    [Test]
    public void 페이즈별_이벤트_발생_확인()
    {
        var sut = CreateSut();

        var flowEvents = new List<GameFlowData>();
        var banEvents = new List<Team>();
        var pickEvents = new List<Team>();
        var swapEvents = new List<Team>();
        var traitEvents = new List<Team>();
        var doneEvents = new List<Team>();

        sut.OnGameProgress += flow => flowEvents.Add(flow);
        sut.OnPhaseBan += (team) => banEvents.Add(team);
        sut.OnPhasePick += (team) => pickEvents.Add(team);
        sut.OnPhaseSwap += (team) => swapEvents.Add(team);
        sut.OnPhaseSkill += (team) => traitEvents.Add(team);
        sut.OnPhaseDone += () => doneEvents.Add(Team.All);

        Dispatch(GamePhase.Ban, Team.Blue);
        Dispatch(GamePhase.Pick,Team.Red);
        Dispatch(GamePhase.Swap, Team.Blue);
        Dispatch(GamePhase.Skill, Team.Red);
        Dispatch(GamePhase.Done, Team.Blue);


        Assert.AreEqual(Team.Blue, banEvents[0]);
        Assert.AreEqual(Team.Red, pickEvents[0]);
        Assert.AreEqual(Team.Blue, swapEvents[0]);
        Assert.AreEqual(Team.Red, traitEvents[0]);

        // 다른 이벤트들은 추가로 발생하지 않았음을 확인
        Assert.AreEqual(1, banEvents.Count);
        Assert.AreEqual(1, pickEvents.Count);
        Assert.AreEqual(1, swapEvents.Count);
        Assert.AreEqual(1, traitEvents.Count);
        Assert.AreEqual(1, doneEvents.Count);

        CollectionAssert.AreEqual(new GameFlowData[]
        {
            CreateFlow(GamePhase.Ban, Team.Blue),
            CreateFlow(GamePhase.Pick, Team.Red),
            CreateFlow(GamePhase.Swap, Team.Blue),
            CreateFlow(GamePhase.Skill, Team.Red),
            CreateFlow(GamePhase.Done, Team.Blue),
        }, 
        flowEvents);

        void Dispatch(GamePhase gamePhase, Team team) => sut.Dispatch(gamePhase, team);
    }

    GameFlowData CreateFlow(GamePhase phase, Team team) => new GameFlowData(phase, team);

    [Test]
    public void All일_경우_모든_팀에_이벤트_호출()
    {
        var sut = CreateSut();
        var events = new List<Team>();
        sut.OnPhaseSwap += (team) => events.Add(team);

        // Act
        sut.Dispatch(GamePhase.Swap, Team.All);

        Assert.AreEqual(2, events.Count);
        CollectionAssert.AreEquivalent(new Team[] { Team.Red, Team.Blue }, events);
    }
}
