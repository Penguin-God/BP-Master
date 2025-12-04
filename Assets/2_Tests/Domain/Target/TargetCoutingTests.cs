using NUnit.Framework;
using static TestHelper;

public class TargetCoutingTests
{
    [Test]
    public void 범위_All은_특정_사이드면_타겟_수는_팀_크기만큼()
    {
        var sut = new TargetCounter(4);

        int result = sut.CalculateTargetCount(OpponentAllRule);

        Assert.AreEqual(4, result);
    }

    [Test]
    public void All은_팀_크기의_2배만큼()
    {
        var sut = new TargetCounter(5);

        int result = sut.CalculateTargetCount(AllRule);

        Assert.AreEqual(10, result);
    }

    [Test]
    public void 숫자_범위는_사이드에_상관없이_크기만큼()
    {
        var sut = new TargetCounter(5);

        Assert.AreEqual(1, sut.CalculateTargetCount(new SkillTargetRule(Side.All, TargetRange.Single)));
        Assert.AreEqual(2, sut.CalculateTargetCount(new SkillTargetRule(Side.Opponent, TargetRange.Double)));
        Assert.AreEqual(3, sut.CalculateTargetCount(new SkillTargetRule(Side.Self, TargetRange.Triple)));
    }


    TeamCounter CreateSut(int blue = 3, int red = 2)
    => new TeamCounter(blue, red);

    SkillTargetRule CreateRule(TargetRange range, Side side) => new SkillTargetRule (side, range);

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

        var countBlue = sut.CalculateTargetCount(Team.Blue, rule);
        var countRed = sut.CalculateTargetCount(Team.Red, rule);

        Assert.AreEqual(5, countBlue);
        Assert.AreEqual(5, countRed);
    }

    [Test]
    public void All범위_Self사이드면_자기팀_카운트_반환()
    {
        var sut = CreateSut(blue: 3, red: 2);
        var rule = CreateRule(TargetRange.All, Side.Self);

        var fromBlue = sut.CalculateTargetCount(Team.Blue, rule);
        var fromRed = sut.CalculateTargetCount(Team.Red, rule);

        Assert.AreEqual(3, fromBlue); // Blue 자기 자신
        Assert.AreEqual(2, fromRed);  // Red 자기 자신
    }

    [Test]
    public void All범위_Opponent사이드면_상대팀_카운트_반환()
    {
        var sut = CreateSut(blue: 3, red: 2);
        var rule = CreateRule(TargetRange.All, Side.Opponent);

        var fromBlue = sut.CalculateTargetCount(Team.Blue, rule);
        var fromRed = sut.CalculateTargetCount(Team.Red, rule);

        Assert.AreEqual(2, fromBlue); // Blue의 상대 = Red
        Assert.AreEqual(3, fromRed);  // Red의 상대 = Blue
    }

    [Test]
    public void Single_Double_Triple_범위는_고정값을_반환()
    {
        var sut = CreateSut(blue: 10, red: 10);

        Assert.AreEqual(1, sut.CalculateTargetCount(
            Team.Blue,
            CreateRule(TargetRange.Single, Side.All)));

        Assert.AreEqual(2, sut.CalculateTargetCount(
            Team.Red,
            CreateRule(TargetRange.Double, Side.Self)));

        Assert.AreEqual(3, sut.CalculateTargetCount(
            Team.Blue,
            CreateRule(TargetRange.Triple, Side.Opponent)));
    }
}
