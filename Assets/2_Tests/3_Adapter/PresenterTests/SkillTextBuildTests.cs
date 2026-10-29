using NUnit.Framework;
using static TestHelper;

public class SkillTextBuildTests
{
    SkillTextBuilder CreateSut(string template = "{Condition} {Rule} {Stat} {Value} {Action}")
    {
        var statChangeData = new StatChangeTextData("증가", "감소", "고정");
        var statTextData = new StatTextData("공격력", "방어력", "속도");
        var sideTextData = new SideTextData("본인 슬롯", "상대 슬롯", "양쪽 슬롯", "대상 없음", "의 모든 카드", "에서 선택한 카드");
        var convertKey = new SkillConvertKey("{Condition}", "{Rule}", "{Stat}", "{Value}", "{Action}");

        return new SkillTextBuilder(template, statChangeData, statTextData, sideTextData, convertKey);
    }

    [Test]
    public void 스킬_텍스트_전체_생성2()
    {
        var sut = CreateSut();
        var amount = CreateSkillAmount(AmountType.Value, statType: StatType.Attack, value: 20);
        var model = CreateSkillViewModel(rule: SelfAllRule, amountData: amount);

        var result = sut.BuildGroupedSkillText(CreateSkillViewModels(model));

        Assert.AreEqual("본인 슬롯의 모든 카드 공격력 20 증가", result);
    }

    [Test]
    public void 스킬_텍스트_전체_생성3()
    {
        var sut = CreateSut();
        var amount = CreateSkillAmount(AmountType.Value, statType: StatType.Attack, value: 20);
        var model = CreateSkillViewModel(rule: SelfDouble, amountData: amount);

        var result = sut.BuildGroupedSkillText(CreateSkillViewModels(model));

        Assert.AreEqual("본인 슬롯에서 선택한 카드 두 장 공격력 20 증가", result);
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

    [Test]
    public void 동일한_조건과_타깃_규칙을_가진_스킬들은_타깃문구가_중복_출력되지_않는다()
    {
        var sut = CreateSut();
        var attackAmount = CreateSkillAmount(AmountType.Fix, statType: StatType.Attack, fix: 100);
        var defenseAmount = CreateSkillAmount(AmountType.Fix, statType: StatType.Defense, fix: 100);

        var skill1 = CreateSkillViewModel(rule: SelfDouble, amountData: attackAmount);
        var skill2 = CreateSkillViewModel(rule: SelfDouble, amountData: defenseAmount);

        var result = sut.BuildGroupedSkillText(new[] { skill1, skill2 });

        Assert.AreEqual("본인 슬롯에서 선택한 카드 두 장 공격력 100 고정, 방어력 100 고정", result);
    }
}