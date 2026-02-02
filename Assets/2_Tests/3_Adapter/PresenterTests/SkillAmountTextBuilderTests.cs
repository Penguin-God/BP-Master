using NUnit.Framework;
using static TestHelper;

public class SkillAmountTextBuilderTests
{
    [Test]
    [TestCase(StatType.Attack, "공격력")]
    [TestCase(StatType.Defense, "방어력")]
    [TestCase(StatType.Speed, "속도")]
    public void 스탯_종류에_따른_텍스트_반환_확인(StatType statType, string expected)
    {
        var sut = CreateBuilder();

        var result = sut.BuildStatText(statType);

        Assert.AreEqual(expected, result);
    }

    [Test]
    [TestCase(AmountType.Value, "100")]
    [TestCase(AmountType.Percent, "50%")]
    [TestCase(AmountType.Fix, "120")]
    public void 타입에_따른_값을_텍스트로_반환(AmountType amountType, string expected)
    {
        SkillAmountData data = CreateSkillAmount(amountType, value: 100, percent: 0.5f, fix: 120);
        var sut = CreateBuilder();

        string result = sut.BuildAmountText(data);

        Assert.AreEqual(expected, result);
    }

    [Test]
    [TestCase(AmountType.Value, "감소")]
    [TestCase(AmountType.Percent, "증가")]
    [TestCase(AmountType.Fix, "고정")]
    public void 값에_따라_변경_텍스트_반환(AmountType amountType, string expected)
    {
        var sut = CreateBuilder();

        SkillAmountData data = CreateSkillAmount(amountType, value: -100, percent: 0.5f, fix: 120);
        string result = sut.BuildChangeText(data);

        Assert.AreEqual(expected, result);
    }

    SkillAmountTextBuilder CreateBuilder()
    {
        var dummyData = new AmountTextData("증가", "감소", "고정");
        return new SkillAmountTextBuilder(dummyData);
    }
}
