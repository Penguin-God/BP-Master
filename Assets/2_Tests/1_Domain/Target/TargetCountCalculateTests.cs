using NUnit.Framework;
using static TestHelper;

public class TargetCountCalculateTests
{
    TargetFindContext CreateSut(Team team, CardSelectRule rule, int blue = 3, int red = 2) => new TargetFindContext(team, rule, new TeamCount(blue, red));
    CardSelectRule CreateRule(TargetRange range, Side side) => new CardSelectRule(side, range);
    int Calculate(TargetFindContext sut) => TargetCountCalculator.CalculateTargetCount(sut);

    [Test]
    public void 팀카운트_팀별_수량_반환()
    {
        var sut = new TeamCount(3, 2);
        Assert.AreEqual(3, sut.GetTeamCount(Team.Blue));
        Assert.AreEqual(2, sut.GetTeamCount(Team.Red));
    }

    [Test]
    public void 타겟_컨텍스트를_기반으로_최대_선택_가능_수량을_올바르게_계산한다()
    {
        var sut = CreateSut(Team.Blue, SelfSingleRule, 3, 2);
        Assert.AreEqual(1, Calculate(sut));
    }

    [Test]
    public void All범위_All사이드면_양팀_전체합을_반환()
    {
        var rule = CreateRule(TargetRange.All, Side.All);
        var sut = CreateSut(Team.Blue, rule, 3, 2);
        Assert.AreEqual(5, Calculate(sut));

        sut = CreateSut(Team.Red, rule, 3, 2);
        Assert.AreEqual(5, Calculate(sut));
    }

    [Test]
    public void All범위_Self사이드면_자기팀_카운트_반환()
    {
        var rule = CreateRule(TargetRange.All, Side.Self);
        var sut = CreateSut(Team.Blue, rule, 3, 2);
        Assert.AreEqual(3, Calculate(sut));

        sut = CreateSut(Team.Red, rule, 3, 2);
        Assert.AreEqual(2, Calculate(sut));
    }

    [Test]
    public void All범위_Opponent사이드면_상대팀_카운트_반환()
    {
        var rule = CreateRule(TargetRange.All, Side.Opponent);
        var sut = CreateSut(Team.Blue, rule, 3, 2);
        Assert.AreEqual(2, Calculate(sut));

        sut = CreateSut(Team.Red, rule, 3, 2);
        Assert.AreEqual(3, Calculate(sut));
    }

    [Test]
    public void Single_Double_Triple_범위는_고정값을_반환()
    {
        var sut = CreateSut(Team.Blue, SelfSingleRule, 10, 10);
        Assert.AreEqual(1, Calculate(sut));

        sut = CreateSut(Team.Red, SelfDouble, 10, 10);
        Assert.AreEqual(2, Calculate(sut));

        sut = CreateSut(Team.Blue, SelfTriple, 10, 10);
        Assert.AreEqual(3, Calculate(sut));
    }

    [Test]
    public void 고정값이_팀_카운트를_넘기면_팀_수를_반환()
    {
        var sut = CreateSut(Team.Blue, OpponentDoubleRule, 2, 1);
        Assert.AreEqual(1, Calculate(sut));
    }
}
