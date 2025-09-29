using NUnit.Framework;

public class TraitApplyTests
{
    [Test]
    public void 조건_만족시_Action_실행됨()
    {
        var champion = TestHelper.CreateStatus(10, def: 60, 5);
        var sut = new TraitExecutor(new TestAttackChangeAction(5), TraitConditionType.DefenseAtLeast, 50);

        sut.ExecuteTrait(champion);

        Assert.AreEqual(15, champion.Stat.Attack);
    }

    [Test]
    public void 조건_불만족시_Action_실행되지않음()
    {
        var champion = TestHelper.CreateStatus(10, 40, 5);
        var sut = new TraitExecutor(new TestAttackChangeAction(5), TraitConditionType.DefenseAtLeast, 50);

        sut.ExecuteTrait(champion);

        Assert.AreEqual(10, champion.Stat.Attack);
    }
}