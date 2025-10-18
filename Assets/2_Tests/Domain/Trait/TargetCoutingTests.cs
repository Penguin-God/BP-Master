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

        Assert.AreEqual(1, sut.CalculateTargetCount(new TraitTargetRule(Side.All, TargetRange.Single)));
        Assert.AreEqual(2, sut.CalculateTargetCount(new TraitTargetRule(Side.Opponent, TargetRange.Double)));
        Assert.AreEqual(3, sut.CalculateTargetCount(new TraitTargetRule(Side.Self, TargetRange.Triple)));
    }
}
