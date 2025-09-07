using NUnit.Framework;
using System.Collections.Generic;

public class TraitControllerTests
{
    [Test]
    public void 특성_적용()
    {
        TraitController sut = new TraitController(null);
        Champion champion = new Champion(0, "", default, null);

        sut.ApplyTrait(new TestAttackChanger(10), new Champion[] { champion });

        Assert.AreEqual(10, champion.StatData.Attack);
    }

    [Test]
    public void 선택한_특성_타겟에_적용()
    {
        Dictionary<Team, IReadOnlyList<Trait>> traits = new();
        traits.Add(Team.Blue, new List<Trait>() { null, new Trait(Side.Opponent, TargetRange.Single, new AttackChanger(10)) });

        TraitController sut = new TraitController(traits);
        Champion champion = new Champion(0, "", default, null);

        sut.ApplyTrait(Team.Blue, 1, new Champion[] { champion });

        Assert.AreEqual(10, champion.StatData.Attack);
    }
}
