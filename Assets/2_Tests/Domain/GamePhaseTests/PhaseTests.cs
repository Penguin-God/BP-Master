using NUnit.Framework;

public class PhaseTests
{
    PhaseData CreateData(GamePhase phase, params Team[] order) => new PhaseData(phase, new Phase(order));
    PhaseManager CreateSut(PhaseData[] phaseDatas) => new PhaseManager(phaseDatas, new PhaseEventDispatcher());

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
    public void Start_호출시_첫_흐름이_발행된다()
    {
        var sut = CreateSut(new[] { CreateData(GamePhase.Ban, Team.Blue) });

        GameFlowData currentFlow = default;
        sut.OnFlowChanged += (f) => currentFlow = f;

        sut.Start();

        Assert.AreEqual(GamePhase.Ban, currentFlow.Phase);
        Assert.AreEqual(Team.Blue, currentFlow.Turn);
    }

    [Test]
    public void 올바른_팀_제출시_다음_흐름_이벤트()
    {
        var sut = CreateSut(new[]
        {
            CreateData(GamePhase.Ban,  Team.Blue, Team.Red),
            CreateData(GamePhase.Pick, Team.Blue)
        });

        GameFlowData currentFlow = default;
        sut.OnFlowChanged += (f) => currentFlow = f;

        sut.Start();

        sut.SubmitAction(Team.Blue);
        Assert.AreEqual(GamePhase.Ban, currentFlow.Phase);
        Assert.AreEqual(Team.Red, currentFlow.Turn);   // 다음 턴으로 이동

        sut.SubmitAction(Team.Red);
        Assert.AreEqual(GamePhase.Pick, currentFlow.Phase);
        Assert.AreEqual(Team.Blue, currentFlow.Turn);   // 다음 페이즈 첫 턴
    }

    [Test]
    public void 잘못된_팀_제출시_이벤트_없음()
    {
        var sut = CreateSut(new[] { CreateData(GamePhase.Ban, Team.Blue) }); // 요구 턴: Blue
        sut.Start();
        int eventCount = 0;
        sut.OnFlowChanged += _ => eventCount++;

        sut.SubmitAction(Team.Red); // 잘못된 팀 제출
        Assert.AreEqual(0, eventCount);
    }

    [Test]
    public void TeamAll_은_양팀_모두_제출해야_진행한다()
    {
        var sut = CreateSut(new[] { CreateData(GamePhase.Swap, Team.All) });

        GameFlowData currentFlow = default;
        sut.OnFlowChanged += (f) => currentFlow = f;
        sut.Start();

        sut.SubmitAction(Team.Blue);
        Assert.AreEqual(GamePhase.Swap, currentFlow.Phase);  // 아직 Swap

        sut.SubmitAction(Team.Red);
        Assert.AreEqual(GamePhase.Done, currentFlow.Phase);  // Done으로 진행
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
