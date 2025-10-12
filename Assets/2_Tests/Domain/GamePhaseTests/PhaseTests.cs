using NUnit.Framework;
using System;

public class PhaseTests
{
    PhaseData CreateData(GamePhase phase, params Team[] order) => new PhaseData(phase, new Phase(order));
    PhaseManager CreateSut(PhaseData[] phaseDatas) => new PhaseManager(phaseDatas, new PhaseEventDispatcher());
    GameFlowData CreateFlow(GamePhase phase, Team team) => new GameFlowData(phase, team);   


    [Test]
    public void 다음턴_반환()
    {
        Team[] teams = new Team[] { Team.Red, Team.Blue };
        Phase phase = new(teams);

        Assert.AreEqual(Team.Red, phase.GetNext());
        Assert.IsFalse(phase.IsDone);
        Assert.AreEqual(Team.Blue, phase.GetNext());
        Assert.IsTrue(phase.IsDone);
    }

    [Test]
    public void 턴_없는데_달라_하면_에러()
    {
        Phase phase = new(new Team[] { Team.Red });

        phase.GetNext();
        Assert.Throws<System.InvalidOperationException>(() => phase.GetNext());
    }

    [Test]
    public void Start_호출시_첫_흐름이_진행()
    {
        var sut = CreateSut(new[] { CreateData(GamePhase.Ban, Team.Blue) });

        sut.Start();

        Assert.AreEqual(CreateFlow(GamePhase.Ban, Team.Blue), sut.CurrentFlow);
    }

    [Test]
    public void 올바른_팀_제출시_다음_흐름_진행()
    {
        var sut = CreateSut(new[]
        {
            CreateData(GamePhase.Ban,  Team.Blue, Team.Red),
            CreateData(GamePhase.Pick, Team.Blue)
        });

        sut.Start();

        sut.SubmitAction(Team.Blue);
        Assert.AreEqual(CreateFlow(GamePhase.Ban, Team.Red), sut.CurrentFlow);

        sut.SubmitAction(Team.Red);
        Assert.AreEqual(CreateFlow(GamePhase.Pick, Team.Blue), sut.CurrentFlow);
    }

    [Test]
    public void 잘못된_팀_제출시_에러()
    {
        var sut = CreateSut(new[] { CreateData(GamePhase.Ban, Team.Blue) });
        sut.Start();

        Assert.Throws<Exception>(() => sut.SubmitAction(Team.Red));
    }

    [Test]
    public void TeamAll_은_양팀_모두_제출해야_진행한다()
    {
        var sut = CreateSut(new[] { CreateData(GamePhase.Swap, Team.All) });
        sut.Start();

        sut.SubmitAction(Team.Blue);
        Assert.AreEqual(GamePhase.Swap, sut.CurrentFlow.Phase);  // 아직 Swap

        sut.SubmitAction(Team.Red);
        Assert.AreEqual(GamePhase.Done, sut.CurrentFlow.Phase);  // Done으로 진행됨
    }


    [Test]
    public void 디스패처에서_이벤트_발생()
    {
        var dispatcher = new PhaseEventDispatcher();
        var sut = new PhaseManager(new[] { CreateData(GamePhase.Ban, Team.Blue), }, dispatcher);
        bool isCall = false;
        dispatcher.OnPhaseBan += _ => isCall = true;

        sut.Start();

        Assert.IsTrue(isCall);
    }
}
