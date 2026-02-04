using NUnit.Framework;
using static TestHelper;
using System;

public class TestActionFactory : ISkillActionFactory
{
    readonly int AttackValue;

    public TestActionFactory(int attackValue) => AttackValue = attackValue;

    public ISkillAction CreateAction(SkillType actionType, SkillAmountData amountData, ChampionStatus caster)
    {
        return new TestAttackChangeAction(AttackValue);
    }
}

public class SkillFactoryTests
{
    [Test]
    [TestCase(100, 0)]
    [TestCase(0, 10)]
    public void 팩토리로_넘긴_값이_조건과_액션에_적용되야_함(int attThreshold, int expected)
    {
        var status = CreateStatus();

        // att가 기준값 이상일 때 Attack +10
        SkillConditionData condition = CreateThresholdCondition(StatConditionType.AttackAtLeast, attThreshold);
        var data = CreateValueSkillData(StatType.Attack, 10, condition, SelfAllRule);
        var result = new SkillExecutorFactory(new TestActionFactory(10)).CreateExecutor(data, status);

        result.ExecuteSkill(new ChampionStatus[] { status });

        Assert.AreEqual(expected, status.Stat.Attack);
    }

    [TestCase(SkillType.TraitExcluder, typeof(SkillExcluder))]
    [TestCase(SkillType.StatAbsorber, typeof(DefenseAbsorber))]
    [TestCase(SkillType.Resonance, typeof(Resonance))]
    [TestCase(SkillType.AmplifyChanger, typeof(AmplifyChanger))]
    [TestCase(SkillType.PickBuffer, typeof(PickChampStatChanger))]
    [TestCase(SkillType.Doppelganger, typeof(Doppelganger))]
    [TestCase(SkillType.FinalStatChanger, typeof(FinalStatChanger))]
    public void Type에_맞는_Action_객체_생성(SkillType type, Type expectedType)
    {
        var result = new SkillActionFactory(new PhaseActionEventDispatcher(), new PhaseEventDispatcher(), Team.Blue).CreateAction(type, CreateSkillAmount(AmountType.Fix, StatType.Attack, 1, 1, 1), CreateStatus());
        result.Do(CreateStatus()); // 에러만 체크
        Assert.IsInstanceOf(expectedType, result);
    }

    [TestCase(ConditionType.None, typeof(NullChecker))]
    [TestCase(ConditionType.Threshold, typeof(StatThresholdChecker))]
    [TestCase(ConditionType.Compare, typeof(StatComparisonChecker))]
    public void 타입에_맞는_조건_객체_생성(ConditionType checkerType, Type expectedType)
    {
        SkillConditionData data = CreateConditionData(checkerType);
        var result = SkillCondtionFactory.CreateCondition(data, CreateStat());
        result.Check(CreateStatus()); // 에러만 체크
        Assert.IsInstanceOf(expectedType, result);
    }


    [Test]
    [TestCase(AmountType.Value, typeof(ValueCalculator))]
    [TestCase(AmountType.Percent, typeof(PercentCalculator))]
    [TestCase(AmountType.Fix, typeof(FixCalculator))]
    public void 타입에_맞는_계산기_객체_생성(AmountType amountType, Type expectedType)
    {
        ISkillAmountCalculator result = SkillAmountCalculatorFactory.Create(CreateSkillAmount(amountType, StatType.Attack, 1, 1, 1));
        result.Calculate(100); // 에러만 체크
        Assert.IsInstanceOf(expectedType, result);
    }
}
