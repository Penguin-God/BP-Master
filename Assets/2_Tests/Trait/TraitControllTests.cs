using NUnit.Framework;
using System.Collections.Generic;

public class TraitControllTests
{
    [Test]
    public void 선택한_특성_타겟에_적용()
    {
        Dictionary<Team, IReadOnlyList<Champion>> data = new();
        data.Add(Team.Blue, new Champion[] { TestHelper.CreateTraitChamp(Side.Opponent, TargetRange.Single, 10) });
        data.Add(Team.Red, new Champion[] { TestHelper.CreateTraitChamp(Side.Opponent, TargetRange.Single, 10) });
        TraitController traitPresenter = new TraitController(data);

        Assert.IsTrue(traitPresenter.UseTrait(Team.Blue, 0, 0));
        Assert.AreEqual(10, data[Team.Red][0].StatData.Attack);
    }

    [Test]
    public void 한_챔피언이_특성_중복_사용_불가()
    {
        Dictionary<Team, IReadOnlyList<Champion>> data = new();
        data.Add(Team.Blue, new Champion[] { TestHelper.CreateTraitChamp(Side.Opponent, TargetRange.Single, 10) });
        data.Add(Team.Red, new Champion[] { TestHelper.CreateTraitChamp(Side.Opponent, TargetRange.Single, 10) });
        TraitController traitPresenter = new TraitController(data);

        Assert.IsTrue(traitPresenter.UseTrait(Team.Blue, 0, 0));
        Assert.AreEqual(10, data[Team.Red][0].StatData.Attack);

        Assert.IsFalse(traitPresenter.UseTrait(Team.Blue, 0, 0));
        Assert.AreEqual(10, data[Team.Red][0].StatData.Attack);
    }

    [Test]
    public void 특성_시전자는_사용_플래그_true()
    {
        var data = new Dictionary<Team, IReadOnlyList<Champion>>
        {
            { Team.Blue, new Champion[] { TestHelper.CreateTraitChamp(Side.Opponent, TargetRange.Single, 10) } },
            { Team.Red,  new Champion[] { TestHelper.CreateTraitChamp(Side.Opponent, TargetRange.Single, 10) } }
        };
        var sut = new TraitController(data);

        // Blue 0번이 사용
        Assert.IsTrue(sut.UseTrait(Team.Blue, 0, 0));

        // 시전자 슬롯만 true
        Assert.IsTrue(sut.IsTraitUsed(TestHelper.CreateBlueSlot(0)));
        Assert.IsFalse(sut.IsTraitUsed(TestHelper.CreateRedSlot(0)));
    }

    [Test]
    public void 시전자와_타겟_인덱스_달라질_때()
    {
        var data = new Dictionary<Team, IReadOnlyList<Champion>>
        {
            {
                Team.Blue,
                new Champion[]
                {
                    TestHelper.CreateTraitChamp(Side.Opponent, TargetRange.Single, 5),
                    TestHelper.CreateTraitChamp(Side.Opponent, TargetRange.Single, 11),
                }
            },
            {
                Team.Red,
                new Champion[]
                {
                    TestHelper.CreateTraitChamp(Side.Opponent, TargetRange.Single, 999) // 사용 안 됨
                }
            }
        };

        var sut = new TraitController(data);

        Assert.IsTrue(sut.UseTrait(Team.Blue, traitIndex: 1, targetIndex: 0));

        // ✅ 기대: Red[0] 공격력은 11이어야 함 (시전자 Blue[1]의 효과)
        Assert.AreEqual(11, data[Team.Red][0].StatData.Attack);

        // ✅ 기대: 사용 플래그는 Blue[1]만 true
        Assert.IsFalse(sut.IsTraitUsed(TestHelper.CreateBlueSlot(0)));
        Assert.IsTrue(sut.IsTraitUsed(TestHelper.CreateBlueSlot(1)));
    }
}
