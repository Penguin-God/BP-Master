using NUnit.Framework;
using System.Collections.Generic;
using static TestHelper;

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
        var doneEvents = new List<Team>();

        sut.OnGameProgress += flow => flowEvents.Add(flow);
        sut.OnPhaseBan += (team) => banEvents.Add(team);
        sut.OnPhasePick += (team) => pickEvents.Add(team);
        sut.OnPhaseDone += () => doneEvents.Add(Team.All);

        Dispatch(GamePhase.Ban, Team.Blue);
        Dispatch(GamePhase.Pick,Team.Red);
        Dispatch(GamePhase.Done, Team.All);


        Assert.AreEqual(Team.Blue, banEvents[0]);
        Assert.AreEqual(Team.Red, pickEvents[0]);

        // 다른 이벤트들은 추가로 발생하지 않았음을 확인
        Assert.AreEqual(1, banEvents.Count);
        Assert.AreEqual(1, pickEvents.Count);
        Assert.AreEqual(1, doneEvents.Count);

        CollectionAssert.AreEqual(new GameFlowData[] { CreateFlow(GamePhase.Ban, Team.Blue), CreateFlow(GamePhase.Pick, Team.Red), CreateFlow(GamePhase.Done, Team.All), }, flowEvents);

        void Dispatch(GamePhase gamePhase, Team team) => sut.Dispatch(gamePhase, team);
    }
}
