using NUnit.Framework;
using static TestHelper;

public class SkillFactoryTests
{
    [Test]
    [TestCase(100, 0)]
    [TestCase(0, 10)]
    public void 팩토리로_넘긴_값이_조건과_액션에_적용되야_함(int attThreshold, int expected)
    {
        var champion = CreateStatus(0);

        // att가 기준값 이상일 때 Attack +10
        SkillConditionData condition = CreateThresholdCondition(StatConditionType.AttackAtLeast, attThreshold);
        var data = CreateTraitData(SkillType.AttackChanger, 10, condition, SelfAllRule);
        var result = new SkillExecutorFactory().CreateExecutor(data, default);

        result.ExecuteSkill(new ChampionStatus[] { champion });

        Assert.AreEqual(expected, champion.Stat.Attack);
    }

    [TestCase(SkillType.AttackChanger, typeof(AttackChanger))]
    [TestCase(SkillType.DefenseChanger, typeof(DefenseChanger))]
    [TestCase(SkillType.SpeedChanger, typeof(SpeedChanger))]
    [TestCase(SkillType.DefenseFixer, typeof(DefenseFixer))]
    [TestCase(SkillType.TraitExcluder, typeof(SkillExcluder))]
    public void Type에_맞는_Action_객체_생성(SkillType type, System.Type expectedType)
    {
        var result = SkillActionFactory.CreateAction(type, 0);
        result.Do(CreateStatus()); // 에러만 체크
        Assert.IsInstanceOf(expectedType, result);
    }


    [TestCase(ConditionType.None, typeof(NullChecker))]
    [TestCase(ConditionType.Threshold, typeof(StatThresholdChecker))]
    [TestCase(ConditionType.Compare, typeof(StatComparisonChecker))]
    public void Data에_따른_조건_검사_객체_생성(ConditionType checkerType, System.Type expectedType)
    {
        SkillConditionData data = new SkillConditionData(StatConditionType.None, 0, checkerType);
        var result = ChampionCondtionFactory.CreateChecker(data, CreateStat());
        Assert.IsInstanceOf(expectedType, result);
    }

    [TestCase(ConditionType.None, typeof(NullChecker))]
    [TestCase(ConditionType.Threshold, typeof(StatThresholdChecker))]
    [TestCase(ConditionType.Compare, typeof(StatComparisonChecker))]
    [TestCase(ConditionType.Trait, typeof(TraitCondition))]
    public void 타입에_맞는_조건_객체_생성(ConditionType checkerType, System.Type expectedType)
    {
        SkillConditionData data = new SkillConditionData(StatConditionType.None, 0, checkerType);
        var result = ChampionCondtionFactory.CreateCondition(data, CreateStatus());
        result.Check(CreateStat()); // 에러만 체크
        Assert.IsInstanceOf(expectedType, result);
    }
}
