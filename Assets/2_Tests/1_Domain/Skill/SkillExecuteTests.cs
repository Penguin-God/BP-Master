using NUnit.Framework;

public class SkillExecuteTests
{
    [Test]
    public void 조건_만족한_챔프만_실행()
    {
        var champions = new ChampionStatus[] { TestHelper.CreateStatus(att:100), TestHelper.CreateStatus(0) };
        
        SkillExecutor.ExecuteSkill(new TestAttackChangeAction(100), new StatThresholdChecker(StatConditionType.AttackAtLeast, 50), champions);

        Assert.AreEqual(200, champions[0].Stat.Attack);
        Assert.AreEqual(0, champions[1].Stat.Attack);
    }
}