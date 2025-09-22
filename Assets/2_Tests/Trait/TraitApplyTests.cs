using NUnit.Framework;
using System.Linq;

public class TraitApplyTests
{
    [Test]
    public void 조건_만족시_Action_실행됨()
    {
        var champion = TestHelper.CreateStatChamp(10, def: 60, 5);
        var sut = new TraitExecutor(new AttackChanger(5), TraitConditionType.DefenseAtLeast, 50);

        sut.ExecuteTrait(champion);

        Assert.AreEqual(15, champion.StatData.Attack);
    }

    [Test]
    public void 조건_불만족시_Action_실행되지않음()
    {
        var champion = TestHelper.CreateStatChamp(10, 40, 5);
        var sut = new TraitExecutor(new AttackChanger(5), TraitConditionType.DefenseAtLeast, 50);

        sut.ExecuteTrait(champion);

        Assert.AreEqual(10, champion.StatData.Attack);
    }

    [Test]
    public void 여러_대상_모두에_적용되고_각_Δ가_반환된다()
    {
        // given
        var controller = new TraitApplier();
        var targets = new[]
        {
            new ChampionStatus(new ChampionStatData(5,  1, 1)),
            new ChampionStatus(new ChampionStatData(10, 2, 2)),
            new ChampionStatus(new ChampionStatData(20, 3, 3)),
        };
        var exec = new TraitExecutor(new AttackChanger(amount: 5), TraitConditionType.None, 0);

        var deltas = controller.UseTrait(exec, targets).ToArray();

        Assert.AreEqual(3, deltas.Length);
        CollectionAssert.AreEqual(new[] { 5, 5, 5 }, deltas.Select(d => d.Attack));
        CollectionAssert.AreEqual(new[] { 0, 0, 0 }, deltas.Select(d => d.Defense));
        CollectionAssert.AreEqual(new[] { 0, 0, 0 }, deltas.Select(d => d.Speed));

        Assert.AreEqual(10, targets[0].StatData.Attack);
        Assert.AreEqual(15, targets[1].StatData.Attack);
        Assert.AreEqual(25, targets[2].StatData.Attack);
    }
}