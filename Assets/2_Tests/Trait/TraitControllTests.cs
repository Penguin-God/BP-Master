using NUnit.Framework;
using System.Collections.Generic;

public class TraitControllTests
{
    [Test]
    public void 선택한_특성_타겟에_적용()
    {
        Dictionary<Team, IReadOnlyList<Champion>> data = new();
        data.Add(Team.Blue, new Champion[] { new Champion(3, "", default, new Trait(Side.Opponent, TargetRange.Single, new TestAttackChanger(10))) });
        data.Add(Team.Red, new Champion[] { new Champion(13, "", default, new Trait(Side.Opponent, TargetRange.Single, new TestAttackChanger(10))) });
        TraitController traitPresenter = new TraitController(data);

        Assert.IsTrue(traitPresenter.UseTrait(Team.Blue, 0, 0));
        Assert.AreEqual(10, data[Team.Red][0].StatData.Attack);
    }

    [Test]
    public void 한_챔피언이_특성_중복_사용_불가()
    {
        Dictionary<Team, IReadOnlyList<Champion>> data = new();
        data.Add(Team.Blue, new Champion[] { new Champion(3, "", default, new Trait(Side.Opponent, TargetRange.Single, new TestAttackChanger(10))) });
        data.Add(Team.Red, new Champion[] { new Champion(13, "", default, new Trait(Side.Opponent, TargetRange.Single, new TestAttackChanger(10))) });
        TraitController traitPresenter = new TraitController(data);

        Assert.IsTrue(traitPresenter.UseTrait(Team.Blue, 0, 0));
        Assert.AreEqual(10, data[Team.Red][0].StatData.Attack);

        Assert.IsFalse(traitPresenter.UseTrait(Team.Blue, 0, 0));
        Assert.AreEqual(10, data[Team.Red][0].StatData.Attack);
    }
}
