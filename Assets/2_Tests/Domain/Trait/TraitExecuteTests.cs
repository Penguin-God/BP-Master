using NUnit.Framework;

public class TraitExecuteTests
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

    [Test]
    public void 특성_적용_제외된_경우_무시()
    {
        var target = TestHelper.CreateStatus(0, 0, 0);
        target.TraitExcluded();
        var sut = new TraitExecutor(new TestAttackChangeAction(5), TraitConditionType.None, 0);

        sut.ExecuteTrait(target);

        Assert.AreEqual(0, target.Stat.Attack);
    }
}