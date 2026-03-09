using NUnit.Framework;
using static TestHelper;

public class FakeTextBuilder : ISkillActionTextBuilder
{
    public string BuildText(SkillType skillType, SkillAmountData data) => "액숀";
}

public class SkillTextBuildTests
{
    [Test]
    public void 스킬_텍스트_전체_생성()
    {
        var sut = new SkillTextBuilder(new FakeTextBuilder());

        var result = sut.BuildSkillText(CreateSkillDatas(CreateValueSkillData(StatType.Attack, 0, rule: SelfAllRule)));

        Assert.AreEqual("아군 전체 액숀", result);
    }

    [Test]
    public void 조건_타입과_인자값에_맞는_텍스트_생성()
    {
        var sut = new SkillConditionTextBuilder();

        string GetText(SkillConditionData conditionData) => sut.BuildConditionText(conditionData);

        Assert.AreEqual("방어력이 자신보다 높은", GetText(CreateConditionData(ConditionType.Compare, statType: StatConditionType.DefenseAtLeast)));
        Assert.AreEqual("방어력이 자신보다 낮은", GetText(CreateConditionData(ConditionType.Compare, statType: StatConditionType.DefenseBelow)));
        Assert.AreEqual("공격력이 자신보다 낮은", GetText(CreateConditionData(ConditionType.Compare, statType: StatConditionType.AttackBelow)));
        Assert.AreEqual("공격력 120 이하인", GetText(CreateConditionData(ConditionType.Threshold, statType: StatConditionType.AttackBelow, 120)));
        Assert.AreEqual("공격력 120 이상인", GetText(CreateConditionData(ConditionType.Threshold, statType: StatConditionType.AttackAtLeast, 120)));
        Assert.AreEqual("방어력 120 이상인", GetText(CreateConditionData(ConditionType.Threshold, statType: StatConditionType.DefenseAtLeast, 120)));
    }
}