using NUnit.Framework;

public class TraitExecuteTests
{
    [Test]
    public void 특성_적용_제외된_경우_무시()
    {
        var target = TestHelper.CreateStatus(0, 0, 0);
        target.TraitExcluded();
        var sut = new SkillExecutor(new TestAttackChangeAction(5), new NullChecker());

        sut.ExecuteSkill(new ChampionStatus[] { target });

        Assert.AreEqual(0, target.Stat.Attack);
    }

    [Test]
    public void 조건_만족한_챔프만_실행()
    {
        var champions = new ChampionStatus[] { TestHelper.CreateStatus(att:100), TestHelper.CreateStatus(0) };
        var sut = new SkillExecutor(new TestAttackChangeAction(100), new StatThresholdChecker(StatConditionType.AttackAtLeast, 50));

        sut.ExecuteSkill(champions);

        Assert.AreEqual(200, champions[0].Stat.Attack);
        Assert.AreEqual(0, champions[1].Stat.Attack);
    }
}