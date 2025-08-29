using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

public class PhaseTests
{
    //[Test]
    //public void 다음턴_반환()
    //{
    //    Team[] teams = new Team[] { Team.Red, Team.Blue };
    //    Phase phase = new(teams);

    //    Assert.AreEqual(Team.Red, phase.GetNext());
    //    Assert.AreEqual(Team.Blue, phase.GetNext());
    //}

    //[Test]
    //public void 턴_없는데_달라_하면_에러()
    //{
    //    Phase phase = new(new Team[] { Team.Red });

    //    phase.GetNext();
    //    Assert.Throws<InvalidOperationException>(() => phase.GetNext());
    //}

    //[Test]
    //public void 다음_게임_흐름_반환()
    //{
    //    PhaseData[] phases = new PhaseData[]
    //    {
    //        new PhaseData(GamePhase.Ban, new Phase(new Team[] { Team.Red, Team.Blue })),
    //        new PhaseData(GamePhase.Pick, new Phase(new Team[] { Team.Red, Team.Blue })),
    //    };

    //    PhaseManager sut = new(phases);

    //    Assert.AreEqual(CreateFlow(GamePhase.Ban, Team.Red), sut.GetNextFlow());
    //    Assert.AreEqual(CreateFlow(GamePhase.Ban, Team.Blue), sut.GetNextFlow());
    //    Assert.AreEqual(CreateFlow(GamePhase.Pick, Team.Red), sut.GetNextFlow());
    //    Assert.AreEqual(CreateFlow(GamePhase.Pick, Team.Blue), sut.GetNextFlow());
    //    Assert.AreEqual(CreateFlow(GamePhase.Done, Team.All), sut.GetNextFlow());
    //}

    //[Test]
    //public void 값이_같으면_동일()
    //{
    //    Assert.AreEqual(CreateFlow(GamePhase.Ban, Team.Red), CreateFlow(GamePhase.Ban, Team.Red));
    //    Assert.AreNotEqual(CreateFlow(GamePhase.Ban, Team.Red), CreateFlow(GamePhase.Pick, Team.Red));
    //}

    //GameFlowData CreateFlow(GamePhase phase, Team turn) => new GameFlowData(phase, turn);


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
        var sut = new PhaseManager(new[]
        {
            CreateData(GamePhase.Ban, Team.Blue) // 요구 턴: Blue
        });

        sut.Start();        // 1회: Ban/Blue
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
}
