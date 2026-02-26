using NUnit.Framework;
using System;
using static TestHelper;

public class PhaseTests
{
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
        Assert.Throws<InvalidOperationException>(() => phase.GetNext());
    }

    [Test]
    public void Start_호출시_첫_흐름이_진행()
    {
        var sut = CreatePhaseManager(CreatePhaseData(GamePhase.Ban, Team.Blue));

        sut.Start();

        Assert.AreEqual(CreateFlow(GamePhase.Ban, Team.Blue), sut.CurrentFlow);
    }

    [Test]
    public void 올바른_팀_제출시_다음_흐름_진행()
    {
        var sut = CreatePhaseManager(CreatePhaseData(GamePhase.Ban, Team.Blue, Team.Red), CreatePhaseData(GamePhase.Pick, Team.Blue));

        sut.Start();

        sut.SubmitAction(Team.Blue);
        Assert.AreEqual(CreateFlow(GamePhase.Ban, Team.Red), sut.CurrentFlow);

        sut.SubmitAction(Team.Red);
        Assert.AreEqual(CreateFlow(GamePhase.Pick, Team.Blue), sut.CurrentFlow);
    }

    [Test]
    public void 디스패처에서_이벤트_발생()
    {
        var dispatcher = new PhaseEventDispatcher();
        bool isCall = false;
        dispatcher.OnPhaseBan += _ => isCall = true;

        var sut = CreatePhaseManager(dispatcher, CreatePhaseData(GamePhase.Ban, Team.Blue));
        sut.Start();

        Assert.IsTrue(isCall);
    }

    [Test]
    public void 페이즈_진입_에이전트_호출()
    {
        var blue = new TestEntry(banCount: 1);
        var sut = new PhaseFlowOrchestrator(CreatePhaseAdvancer(CreatePhaseData(GamePhase.Ban, Team.Blue, Team.Blue)), new PhaseEventDispatcher(), new TeamPhaseEntryDispatcher(blue, new TestEntry()));
        sut.Start();
        sut.SubmitAction(Team.Blue);

        Assert.AreEqual(2, blue.Count);
    }

    [Test]
    public void Done에_진입하면_마지막_이밴트_실행()
    {
        bool isEnd = false;
        var sut = new PhaseFlowOrchestrator(CreatePhaseAdvancer(CreatePhaseData(GamePhase.Done, Team.Blue)), new PhaseEventDispatcher(), new TeamPhaseEntryDispatcher(new TestEntry(), new TestEntry()));
        sut.OnGameEnd += () => isEnd = true;
        
        sut.Start();

        Assert.IsTrue(isEnd);
    }
}
