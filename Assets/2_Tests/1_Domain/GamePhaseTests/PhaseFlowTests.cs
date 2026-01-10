using NUnit.Framework;
using System;
using static TestHelper;

[TestFixture]
public class PhaseFlowTests
{
    PhaseAdvancer CreateAndStartAdvancer(PhaseData[] phaseDatas)
    {
        var sut = new PhaseAdvancer(phaseDatas);
        sut.Start();
        return sut;
    }

    [Test]
    public void Start_첫_페이즈_첫_턴으로_진행()
    {
        var phases = new[] {CreatePhaseData(GamePhase.Ban, Team.Blue, Team.Red)};
        var sut = CreateAndStartAdvancer(phases);

        Assert.AreEqual(GamePhase.Ban, sut.CurrentFlow.Phase);
        Assert.AreEqual(Team.Blue, sut.CurrentFlow.Turn);
    }

    [Test]
    public void SubmitAction_올바른_팀이면_다음_턴으로_진행()
    {
        var phases = new[] {CreatePhaseData(GamePhase.Ban, Team.Blue, Team.Red)};
        var sut = CreateAndStartAdvancer(phases);

        sut.TryAdvance(Team.Blue);

        Assert.AreEqual(Team.Red, sut.CurrentTurn);
    }

    [Test]
    public void 턴이_끝나면_다음_페이즈로_진행()
    {
        var phases = new[]
        {
            CreatePhaseData(GamePhase.Ban, Team.Blue),
            CreatePhaseData(GamePhase.Pick, Team.Red)
        };

        var sut = CreateAndStartAdvancer(phases);

        sut.TryAdvance(Team.Blue); // Ban 끝 → Pick 시작

        Assert.AreEqual(GamePhase.Pick, sut.CurrentFlow.Phase);
        Assert.AreEqual(Team.Red, sut.CurrentFlow.Turn);
    }

    [Test]
    public void 잘못된_팀이_SubmitAction하면_예외()
    {
        var phases = new[]{CreatePhaseData(GamePhase.Ban, Team.Blue)};
        var sut = CreateAndStartAdvancer(phases);
        Assert.Throws<Exception>(() => sut.TryAdvance(Team.Red));
    }
}
