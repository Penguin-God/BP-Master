using NUnit.Framework;

public class TraitFactoryTests
{
    [Test]
    public void 팩토리로_생성된_특성_조건_참이면_실행()
    {
        var champion = TestHelper.CreateStatus(10, 5, 3);

        // 조건: AttackAtLeast 10 이상일 때만 동작, 액션: Attack +5
        var data = TestHelper.CreateTraitData(TraitType.AttackChanger, 5, TraitConditionType.AttackAtLeast, 10);
        var executor = TraitExecutorFactory.CreateExecutor(data);

        executor.ExecuteTrait(champion);

        Assert.AreEqual(15, champion.Stat.Attack);
        Assert.AreEqual(5, champion.Stat.Defense);
        Assert.AreEqual(3, champion.Stat.Speed);
    }

    [Test]
    public void 팩토리로_생성된_특성_조건_거짓이면_무시()
    {
        var champion = TestHelper.CreateStatus(8, 4, 2);

        // 조건: DefenseAtLeast 10 이상이어야 하지만 현재 4 → 실행되지 않아야 함
        var data = TestHelper.CreateTraitData(TraitType.DefenseChanger, 10, TraitConditionType.DefenseAtLeast, 10);
        var executor = TraitExecutorFactory.CreateExecutor(data);

        executor.ExecuteTrait(champion);

        Assert.AreEqual(8, champion.Stat.Attack);
        Assert.AreEqual(4, champion.Stat.Defense); // unchanged
        Assert.AreEqual(2, champion.Stat.Speed);
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
