using NUnit.Framework;
using System.Collections.Generic;

public class TraitControllTests
{
    [Test]
    public void 특성_선택_안하고_특성_사용시_False()
    {
        var data = new Dictionary<Team, IReadOnlyList<Champion>>();
        data.Add(Team.Blue, new Champion[] { new Champion(3, "", default, new Trait(Side.Opponent, TargetRange.Single, new TestAttackChanger(10))) });
        TraitController traitPresenter = new TraitController(data);

        Assert.IsFalse(traitPresenter.UseTrait(Team.Blue, 1));
    }

    [Test]
    public void 선택한_특성_적용_후_특성_비우기()
    {
        Dictionary<Team, IReadOnlyList<Champion>> data = new();
        data.Add(Team.Blue, new Champion[] { new Champion(3, "", default, new Trait(Side.Opponent, TargetRange.Single, new TestAttackChanger(10))) });
        data.Add(Team.Red, new Champion[] { new Champion(13, "", default, new Trait(Side.Opponent, TargetRange.Single, new TestAttackChanger(10))) });
        TraitController traitPresenter = new TraitController(data);

        Assert.IsFalse(traitPresenter.IsSelected);
        traitPresenter.SelectTrait(Team.Blue, 0);
        Assert.IsTrue(traitPresenter.IsSelected);

        Assert.IsTrue(traitPresenter.UseTrait(Team.Blue, 0));
        Assert.AreEqual(10, data[Team.Red][0].StatData.Attack);
        Assert.IsFalse(traitPresenter.IsSelected);
    }
}
