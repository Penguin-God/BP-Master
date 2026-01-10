using NUnit.Framework;
using static TestHelper;

public class TargetCountCalculateTests
{
    TargetCountCalculator CreateSut(int blue = 3, int red = 2) => new TargetCountCalculator(blue, red);

    SkillTargetRule CreateRule(TargetRange range, Side side) => new SkillTargetRule (side, range);
    int CalculateTargetCount(TargetCountCalculator sut, Team team, SkillTargetRule targetRule) => sut.CalculateTargetCount(team, targetRule);

    [Test]
    public void GetTeamCount_팀별_카운트_정상반환()
    {
        var sut = CreateSut(blue: 3, red: 2);

        Assert.AreEqual(3, sut.GetTeamCount(Team.Blue));
        Assert.AreEqual(2, sut.GetTeamCount(Team.Red));
    }

    [Test]
    public void All범위_All사이드면_양팀_전체합을_반환()
    {
        var sut = CreateSut(blue: 3, red: 2);
        var rule = CreateRule(TargetRange.All, Side.All);

        var countBlue = CalculateTargetCount(sut, Team.Blue, rule);
        var countRed = CalculateTargetCount(sut, Team.Red, rule);

        Assert.AreEqual(5, countBlue);
        Assert.AreEqual(5, countRed);
    }

    [Test]
    public void All범위_Self사이드면_자기팀_카운트_반환()
    {
        var sut = CreateSut(blue: 3, red: 2);
        var rule = CreateRule(TargetRange.All, Side.Self);

        var fromBlue = CalculateTargetCount(sut, Team.Blue, rule);
        var fromRed = CalculateTargetCount(sut, Team.Red, rule);

        Assert.AreEqual(3, fromBlue); // Blue 자기 자신
        Assert.AreEqual(2, fromRed);  // Red 자기 자신
    }

    [Test]
    public void All범위_Opponent사이드면_상대팀_카운트_반환()
    {
        var sut = CreateSut(blue: 3, red: 2);
        var rule = CreateRule(TargetRange.All, Side.Opponent);

        var fromBlue = CalculateTargetCount(sut, Team.Blue, rule);
        var fromRed = CalculateTargetCount(sut, Team.Red, rule);

        Assert.AreEqual(2, fromBlue); // Blue의 상대 = Red
        Assert.AreEqual(3, fromRed);  // Red의 상대 = Blue
    }

    [Test]
    public void Single_Double_Triple_범위는_고정값을_반환()
    {
        var sut = CreateSut(blue: 10, red: 10);

        Assert.AreEqual(1, CalculateTargetCount(sut, Team.Blue, SelfSingleRule));
        Assert.AreEqual(2, CalculateTargetCount(sut, Team.Red, SelfDouble));
        Assert.AreEqual(3, CalculateTargetCount(sut, Team.Blue, SelfTriple));
    }

    [Test]
    public void 고정값이_팀_카운트를_넘기면_팀_수를_반환()
    {
        var sut = CreateSut(blue: 2, red: 1);

        Assert.AreEqual(1, CalculateTargetCount(sut, Team.Blue, OpponentDoubleRule));
    }
}
