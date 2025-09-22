using NUnit.Framework;

public class TraitApplyTests
{
    [Test]
    public void 조건_만족시_Action_실행됨()
    {
        var champion = TestHelper.CreateStatChamp(10, def: 60, 5);
        var traitData = new TraitData(new TestAttackChangerAction(5), TraitConditionType.DefenseAtLeast, 50);
        var sut = new TraitExecutor(traitData);

        sut.ExecuteTrait(champion);

        Assert.AreEqual(15, champion.StatData.Attack);
    }

    [Test]
    public void 조건_불만족시_Action_실행되지않음()
    {
        var champion = TestHelper.CreateStatChamp(10, 40, 5);
        var traitData = new TraitData(new TestAttackChangerAction(5), TraitConditionType.DefenseAtLeast, 50);
        var sut = new TraitExecutor(traitData);

        sut.ExecuteTrait(champion);

        Assert.AreEqual(10, champion.StatData.Attack);
    }
}