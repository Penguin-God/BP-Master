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
