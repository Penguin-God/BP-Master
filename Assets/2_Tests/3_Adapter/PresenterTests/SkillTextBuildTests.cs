using NUnit.Framework;
using static TestHelper;

public class SkillTextBuildTests
{
    SkillTextBuilder CreateSut(string template = "{Condition} {Rule} {Stat} {Value} {Action}")
    {
        var statChangeData = new StatChangeTextData("증가", "감소", "고정");
        var statTextData = new StatTextData("공격력", "방어력", "속도");
        var sideTextData = new SideTextData("본인 슬롯", "상대 슬롯", "양쪽 슬롯", "대상 없음", "의 모든 카드", "에서 선택한 카드 ", "의");
        var convertKey = new SkillConvertKey("{Condition}", "{Rule}", "{Stat}", "{Value}", "{Action}");

        return new SkillTextBuilder(template, statChangeData, statTextData, sideTextData, convertKey);
    }

    [Test]
    public void 스킬_텍스트_전체_생성2()
    {
        var sut = CreateSut();
        var amount = CreateSkillAmount(AmountType.Value, statType: StatType.Attack, value: 20);
        var model = CreateSkillViewModel(rule: SelfAllRule, amountData: amount);

        var result = sut.BuildSkillText(CreateSkillViewModels(model));

        Assert.AreEqual("본인 슬롯의 모든 카드 공격력 20 증가", result);
    }

    [Test]
    public void 스킬_텍스트_전체_생성3()
    {
        var sut = CreateSut();
        var amount = CreateSkillAmount(AmountType.Value, statType: StatType.Attack, value: 20);
        var model = CreateSkillViewModel(rule: SelfDouble, amountData: amount);

        var result = sut.BuildSkillText(CreateSkillViewModels(model));

        Assert.AreEqual("본인 슬롯에서 선택한 카드 둘의 공격력 20 증가", result);
    }

    [Test]
    public void 조건_타입과_인자값에_맞는_텍스트_생성()
    {
        string GetText(SkillConditionData conditionData) => SkillConditionTextBuilder.BuildConditionText(conditionData);

        Assert.AreEqual("방어력이 자신보다 높은", GetText(CreateConditionData(ConditionType.Compare, statType: StatConditionType.DefenseAtLeast)));
        Assert.AreEqual("방어력이 자신보다 낮은", GetText(CreateConditionData(ConditionType.Compare, statType: StatConditionType.DefenseBelow)));
        Assert.AreEqual("공격력이 자신보다 낮은", GetText(CreateConditionData(ConditionType.Compare, statType: StatConditionType.AttackBelow)));
        Assert.AreEqual("공격력 120 이하인", GetText(CreateConditionData(ConditionType.Threshold, statType: StatConditionType.AttackBelow, 120)));
        Assert.AreEqual("공격력 120 이상인", GetText(CreateConditionData(ConditionType.Threshold, statType: StatConditionType.AttackAtLeast, 120)));
        Assert.AreEqual("방어력 120 이상인", GetText(CreateConditionData(ConditionType.Threshold, statType: StatConditionType.DefenseAtLeast, 120)));
    }
}