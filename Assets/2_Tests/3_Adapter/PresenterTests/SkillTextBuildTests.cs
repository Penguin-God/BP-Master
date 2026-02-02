using NUnit.Framework;
using static TestHelper;

public class TestTextBuilder : ISkillActionTextBuilder
{
    public string BuildText(SkillType skillType, SkillAmountData data) => "액숀";
}

public class SkillTextBuildTests
{
    SkillUI_Data CreateData(SkillType traitType, SkillAmountData amountData, SkillConditionData condition, SkillTargetRule rule) => new SkillUI_Data(new SkillData(traitType, amountData, condition, rule));
    [Test]
    public void 스킬_텍스트_전체_생성()
    {
        var sut = new SkillTextBuilder(new TestTextBuilder());
        
        string GetSkillText(AmountType amountType) => sut.BuildSkillText(CreateData(SkillType.StatChanger, CreateSkillAmount(amountType, value: 10, percent: 0.5f, fix: 100), default, SelfAllRule));

        Assert.AreEqual("아군 전체 액숀", GetSkillText(AmountType.Value));
    }

    [Test]
    [TestCase(AmountType.Value, "100")]
    [TestCase(AmountType.Percent, "50%")]
    [TestCase(AmountType.Fix, "120")]
    public void 타입에_따른_값을_텍스트로_반환(AmountType amountType, string expected)
    {
        SkillAmountData data = CreateSkillAmount(amountType, value: 100, percent: 0.5f, fix: 120);
        var sut = new SkillAmountTextBuilder(default);

        string result = sut.BuildAmountText(data);

        Assert.AreEqual(expected, result);
    }

    [Test]
    [TestCase(AmountType.Value, "감소")]
    [TestCase(AmountType.Percent, "증가")]
    [TestCase(AmountType.Fix, "고정")]
    public void 값에_따라_변경_텍스트_반환(AmountType amountType, string expected)
    {
        var sut = new SkillAmountTextBuilder(new AmountChangeTextModel("증가", "감소", "고정"));

        SkillAmountData data = CreateSkillAmount(amountType, value: -100, percent: 0.5f, fix: 120);
        string result = sut.BuildChangeText(data);

        Assert.AreEqual(expected, result);
    }

    [Test]
    public void 조건_타입과_인자값에_맞는_텍스트_생성()
    {
        var sut = new SkillConditionTextBuilder();

        // 편의 함수
        string GetText(SkillConditionData conditionData) => sut.BuildConditionText(conditionData);

        Assert.AreEqual("방어력이 자신보다 높은", GetText(CreateConditionData(ConditionType.Compare, statType: StatConditionType.DefenseAtLeast)));
        Assert.AreEqual("방어력이 자신보다 낮은", GetText(CreateConditionData(ConditionType.Compare, statType: StatConditionType.DefenseBelow)));
        Assert.AreEqual("공격력이 자신보다 낮은", GetText(CreateConditionData(ConditionType.Compare, statType: StatConditionType.AttackBelow)));
        Assert.AreEqual("공격력 120 이하인", GetText(CreateConditionData(ConditionType.Threshold, statType: StatConditionType.AttackBelow, 120)));
        Assert.AreEqual("공격력 120 이상인", GetText(CreateConditionData(ConditionType.Threshold, statType: StatConditionType.AttackAtLeast, 120)));
        Assert.AreEqual("방어력 120 이상인", GetText(CreateConditionData(ConditionType.Threshold, statType: StatConditionType.DefenseAtLeast, 120)));
    }
}
