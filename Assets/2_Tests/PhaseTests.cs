using NUnit.Framework;
using System.Collections.Generic;

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
        Assert.Throws<System.InvalidOperationException>(() => phase.GetNext());
    }

    [Test]
    public void GameFlowData_동치성()
    {
        Assert.AreEqual(CreateFlow(GamePhase.Ban, Team.Red), CreateFlow(GamePhase.Ban, Team.Red));
        Assert.AreNotEqual(CreateFlow(GamePhase.Ban, Team.Red), CreateFlow(GamePhase.Pick, Team.Red));
    }

    GameFlowData CreateFlow(GamePhase phase, Team turn) => new GameFlowData(phase, turn);


    private static PhaseData CreateData(GamePhase phase, params Team[] order) => new PhaseData(phase, new Phase(order));

    [Test]
    public void Start_호출시_첫_흐름이_발행된다()
    {
        var sut = new PhaseManager(new[] { CreateData(GamePhase.Ban, Team.Blue) });

        GameFlowData currentFlow = default;
        sut.OnFlowChanged += (f) => currentFlow = f;

        sut.Start();

        Assert.AreEqual(GamePhase.Ban, currentFlow.Phase);
        Assert.AreEqual(Team.Blue, currentFlow.Turn);
    }

    [Test]
    public void 올바른_팀_제출시_다음_흐름_이벤트()
    {
        var sut = new PhaseManager(new[]
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
        var sut = new PhaseManager(new[] { CreateData(GamePhase.Ban, Team.Blue) }); // 요구 턴: Blue
        sut.Start();
        int eventCount = 0;
        sut.OnFlowChanged += _ => eventCount++;

        sut.SubmitAction(Team.Red); // 잘못된 팀 제출
        Assert.AreEqual(0, eventCount);
    }

    [Test]
    public void TeamAll_은_양팀_모두_제출해야_진행한다()
    {
        var sut = new PhaseManager(new[] { CreateData(GamePhase.Swap, Team.All) });

        GameFlowData currentFlow = default;
        sut.OnFlowChanged += (f) => currentFlow = f;
        sut.Start();

        sut.SubmitAction(Team.Blue);
        Assert.AreEqual(GamePhase.Swap, currentFlow.Phase);  // 아직 Swap

        sut.SubmitAction(Team.Red);
        Assert.AreEqual(GamePhase.Done, currentFlow.Phase);  // Done으로 진행
    }


    [Test]
    public void 페이즈별_이벤트_발생_확인()
    {
        var sut = new PhaseManager(new[]
        {
            CreateData(GamePhase.Ban, Team.Blue),
            CreateData(GamePhase.Pick, Team.Red),
            CreateData(GamePhase.Swap, Team.All),
            CreateData(GamePhase.Trait, Team.Blue)
        });

        // 이벤트 수신 대기
        var banEvents = new List<Team>();
        var pickEvents = new List<Team>();
        var swapEvents = new List<Team>();
        var traitEvents = new List<Team>();
        var doneEvents = new List<Team>();

        sut.OnPhaseBan += (team) => banEvents.Add(team);
        sut.OnPhasePick += (team) => pickEvents.Add(team);
        sut.OnPhaseSwap += (team) => swapEvents.Add(team);
        sut.OnPhaseTrait += (team) => traitEvents.Add(team);
        sut.OnPhaseDone += () => doneEvents.Add(Team.All);

        // 테스트 실행
        sut.Start(); // Ban/Blue 이벤트 발생
        sut.SubmitAction(Team.Blue); // Pick/Red 이벤트 발생
        sut.SubmitAction(Team.Blue);
        sut.SubmitAction(Team.Red);
        sut.SubmitAction(Team.Blue);
        sut.SubmitAction(Team.Red); // Trait/Blue 이벤트 발생
        sut.SubmitAction(Team.Blue); // Done/All 이벤트 발생


        Assert.AreEqual(1, banEvents.Count);
        Assert.AreEqual(Team.Blue, banEvents[0]);

        Assert.AreEqual(1, pickEvents.Count);
        Assert.AreEqual(Team.Red, pickEvents[0]);

        Assert.AreEqual(1, swapEvents.Count);

        Assert.AreEqual(Team.All, swapEvents[0]);
        Assert.AreEqual(1, traitEvents.Count);

        Assert.AreEqual(Team.Blue, traitEvents[0]); // 끝

        // 다른 이벤트들은 추가로 발생하지 않았음을 확인
        Assert.AreEqual(1, banEvents.Count);
        Assert.AreEqual(1, pickEvents.Count);
        Assert.AreEqual(1, swapEvents.Count);
        Assert.AreEqual(1, traitEvents.Count);
        Assert.AreEqual(1, doneEvents.Count);
    }

    [Test]
    public void 동일_페이즈_여러_턴_이벤트_발생()
    {
        var sut = new PhaseManager(new[] { CreateData(GamePhase.Ban, Team.Blue, Team.Red) });

        var banEvents = new List<Team>();
        sut.OnPhaseBan += (team) => banEvents.Add(team);

        sut.Start(); // Blue 턴
        sut.SubmitAction(Team.Blue); // Red 턴

        // Ban 이벤트가 2번 발생해야 함
        Assert.AreEqual(2, banEvents.Count);
        Assert.AreEqual(Team.Blue, banEvents[0]);
        Assert.AreEqual(Team.Red, banEvents[1]);
    }
}
