using NUnit.Framework;
using static TestHelper;

public class TraitFactoryTests
{
    [Test]
    [TestCase(100, 15)]
    [TestCase(0, 10)]
    public void 팩토리로_넘긴_값이_조건과_액션에_적용되야_함(int attThreshold, int expected)
    {
        var champion = CreateStatus(10);

        // att가 기준값 이상일 때 Attack +5
        TraitConditionData condition = CreateThresholdCondition(TraitConditionType.AttackAtLeast, attThreshold);
        var data = CreateTraitData(TraitType.AttackChanger, 5, condition, SelfAllRule);
        var result = TraitExecutorFactory.CreateExecutor(data, default);

        result.ExecuteTrait(champion);

        Assert.AreEqual(expected, champion.Stat.Attack);
    }

    [TestCase(TraitType.AttackChanger, typeof(AttackChanger))]
    [TestCase(TraitType.DefenseChanger, typeof(DefenseChanger))]
    [TestCase(TraitType.SpeedChanger, typeof(SpeedChanger))]
    [TestCase(TraitType.DefenseFixer, typeof(DefenseFixer))]
    [TestCase(TraitType.TraitExcluder, typeof(TraitExcluder))]
    public void Type에_맞는_Action_객체_생성(TraitType type, System.Type expectedType)
    {
        var result = TraitActionFactory.CreateAction(type, 0);
        Assert.IsInstanceOf(expectedType, result);
    }


    [TestCase(ConditionCheckerType.None, typeof(NullChecker))]
    [TestCase(ConditionCheckerType.Threshold, typeof(StatThresholdChecker))]
    [TestCase(ConditionCheckerType.Compare, typeof(StatComparisonChecker))]
    public void Data에_따른_조건_검사_객체_생성(ConditionCheckerType checkerType, System.Type expectedType)
    {
        TraitConditionData data = new TraitConditionData(TraitConditionType.None, 0, checkerType);
        var result = TraitCondtionCheckerFactory.CreateChecker(data, default);
        Assert.IsInstanceOf(expectedType, result);
    }
}
