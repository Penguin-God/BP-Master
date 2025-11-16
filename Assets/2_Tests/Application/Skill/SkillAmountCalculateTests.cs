using NUnit.Framework;

public class SkillAmountCalculateTests
{
    [Test]
    public void 현재값과_무관하게_항상_고정값을_반환()
    {
        ISkillAmountCalculator sut = new ValueCalculator(10);
        Assert.AreEqual(10, sut.Calculate(1000));
    }

    [Test]
    public void 현재값에_퍼센트를_곱해_반올림해_반환()
    {
        ISkillAmountCalculator sut = new PercentCalculator(0.1f);

        Assert.AreEqual(10, sut.Calculate(100));
        Assert.AreEqual(-3, sut.Calculate(-25));
    }
}
