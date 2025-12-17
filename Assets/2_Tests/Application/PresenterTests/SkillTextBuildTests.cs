using NUnit.Framework;
using static TestHelper;

public class SkillTextBuildTests
{
    SkillUI_Data CreateData(SkillType traitType, int amount, SkillConditionData condition, SkillTargetRule rule) => new SkillUI_Data(new SkillData(traitType, new SkillAmountData(AmountType.Value, 0, 0, 0), condition, rule));
    SkillUI_Data CreateData(SkillType traitType, SkillAmountData amountData, SkillConditionData condition, SkillTargetRule rule) => new SkillUI_Data(new SkillData(traitType, amountData, condition, rule));
    [Test]
    public void 특성_액션에_맞는_텍스트_생성()
    {
        var sut = new SkillTextBuilder();
        
        string GetTraitText(AmountType amountType) => sut.BuildSkillText(CreateData(SkillType.AttackChanger, new SkillAmountData(amountType, 10, 0.5f, 100), default, SelfAllRule));

        Assert.AreEqual("아군 전체 공격력 10 증가", GetTraitText(AmountType.Value));
        Assert.AreEqual("아군 전체 공격력 100으로 고정", GetTraitText(AmountType.Fix));
        Assert.AreEqual("아군 전체 공격력 50% 증가", GetTraitText(AmountType.Percent));
    }

    [Test]
    [TestCase(AmountType.Value, "100")]
    [TestCase(AmountType.Percent, "50%")]
    [TestCase(AmountType.Fix, "120")]
    public void 값_타입에_따라_텍스트반환(AmountType amountType, string expected)
    {
        SkillAmountData data = new SkillAmountData(amountType, 100, 0.5f, 120);
        var sut = new SkillAmountTextBuilder();

        string result = sut.BuildAmountText(data);

        Assert.AreEqual(expected, result);
    }

    //[Test]
    //public void 스킬_텍스트는_조건_타겟_액션의_조합()
    //{
    //    var sut = new SkillTextBuilder();

    //    // 편의 함수
    //    string GetTraitText(StatConditionType conditionType, int thershold) 
    //        => sut.BuildSkillText(CreateData(SkillType.DefenseChanger, -10, CreateThresholdCondition(conditionType, thershold), OpponentAllRule));

    //    Assert.AreEqual("방어력 100 이상인 적군 전체 방어력 10 감소", GetTraitText(StatConditionType.DefenseAtLeast, 100));
    //    Assert.AreEqual("방어력 10 이하인 적군 전체 방어력 10 감소", GetTraitText(StatConditionType.DefenseBelow, 10));
    //    Assert.AreEqual("공격력 100 이상인 적군 전체 방어력 10 감소", GetTraitText(StatConditionType.AttackAtLeast, 100));
    //    Assert.AreEqual("공격력 120 이하인 적군 전체 방어력 10 감소", GetTraitText(StatConditionType.AttackBelow, 120));
    //}

    //[Test]
    //public void 조건_타입과_인자값에_맞는_텍스트_생성()
    //{
    //    var sut = new SkillConditionTextBuilder();

    //    // 편의 함수
    //    string GetText(SkillConditionData conditionData) => sut.BuildConditionText(conditionData);

    //    Assert.AreEqual("특성이 가드인", GetText(CreateConditionData(ConditionType.Trait, traitType:TraitType.Guard)));
    //    Assert.AreEqual("방어력이 자신보다 높은", GetText(CreateConditionData(ConditionType.Compare, statType: StatConditionType.DefenseAtLeast)));
    //    Assert.AreEqual("방어력이 자신보다 낮은", GetText(CreateConditionData(ConditionType.Compare, statType: StatConditionType.DefenseBelow)));
    //    Assert.AreEqual("공격력이 자신보다 낮은", GetText(CreateConditionData(ConditionType.Compare, statType: StatConditionType.AttackBelow)));
    //    Assert.AreEqual("공격력 120 이하인", GetText(CreateConditionData(ConditionType.Threshold, statType: StatConditionType.AttackBelow, 120)));
    //    Assert.AreEqual("공격력 120 이상인", GetText(CreateConditionData(ConditionType.Threshold, statType: StatConditionType.AttackAtLeast, 120)));
    //    Assert.AreEqual("방어력 120 이상인", GetText(CreateConditionData(ConditionType.Threshold, statType: StatConditionType.DefenseAtLeast, 120)));
    //}


    //[Test]
    //public void 특성_컬랙션은_텍스트_합쳐서_반환()
    //{
    //    var sut = new SkillTextBuilder();
    //    var datas = new TraitUI_Data[]
    //    {
    //        CreateData(SkillType.AttackChanger, -10, default, OpponentAllRule),
    //        CreateData(SkillType.DefenseChanger, -10, default, OpponentAllRule),
    //    };

    //    string result = sut.BuildSkillText(datas);

    //    Assert.AreEqual("적군 전체 공격력 10 감소, 적군 전체 방어력 10 감소", result);
    //}
}
