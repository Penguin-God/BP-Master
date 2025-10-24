using NUnit.Framework;

public class TraitExecuteTests
{
    [Test]
    [TestCase(100, 10)]
    [TestCase(0, 20)]
    public void 조건_검사_결과에_따라_실행(int threshold, int expected)
    {
        var champion = TestHelper.CreateStatus(10);
        var sut = new TraitExecutor(new TestAttackChangeAction(10), new StatThresholdChecker(TraitConditionType.AttackAtLeast, threshold));

        sut.ExecuteTrait(champion);

        Assert.AreEqual(expected, champion.Stat.Attack);
    }

    [Test]
    public void 특성_적용_제외된_경우_무시()
    {
        var target = TestHelper.CreateStatus(0, 0, 0);
        target.TraitExcluded();
        var sut = new TraitExecutor(new TestAttackChangeAction(5), new NullChecker());

        sut.ExecuteTrait(target);

        Assert.AreEqual(0, target.Stat.Attack);
    }

    [Test]
    public void 조건_만족한_챔프만_실행()
    {
        var champions = new ChampionStatus[] { TestHelper.CreateStatus(att:100), TestHelper.CreateStatus(0) };
        var sut = new TraitExecutor(new TestAttackChangeAction(100), new StatThresholdChecker(TraitConditionType.AttackAtLeast, 50));

        sut.ExecuteTrait(champions);

        Assert.AreEqual(200, champions[0].Stat.Attack);
        Assert.AreEqual(0, champions[1].Stat.Attack);
    }
}