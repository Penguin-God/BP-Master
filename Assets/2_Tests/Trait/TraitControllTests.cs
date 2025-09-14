using NUnit.Framework;
using System.Collections.Generic;

public class TraitControllTests
{
    [Test]
    public void 한_챔피언이_특성_중복_사용_불가()
    {
        Dictionary<Team, IReadOnlyList<Champion>> data = new();
        data.Add(Team.Blue, new Champion[] { TestHelper.CreateTraitChamp(Side.Opponent, TargetRange.Single, 10) });
        data.Add(Team.Red, new Champion[] { TestHelper.CreateTraitChamp(Side.Opponent, TargetRange.Single, 10) });
        TraitController traitPresenter = new TraitController(data);

        Assert.IsTrue(traitPresenter.UseTrait(TestHelper.CreateBlueSlot(0), TestHelper.CreateRedSlot(0)));
        Assert.AreEqual(10, data[Team.Red][0].StatData.Attack);

        Assert.IsFalse(traitPresenter.UseTrait(TestHelper.CreateBlueSlot(0), TestHelper.CreateRedSlot(0)));
        Assert.AreEqual(10, data[Team.Red][0].StatData.Attack);
    }

    [Test]
    public void 특성_시전_후_사용_플래그_true()
    {
        var data = new Dictionary<Team, IReadOnlyList<Champion>>
        {
            { Team.Blue, new Champion[] { TestHelper.CreateTraitChamp(Side.Opponent, TargetRange.Single, 10) } },
            { Team.Red,  new Champion[] { TestHelper.CreateTraitChamp(Side.Opponent, TargetRange.Single, 10) } }
        };
        var sut = new TraitController(data);

        Assert.IsTrue(sut.UseTrait(TestHelper.CreateBlueSlot(0), TestHelper.CreateRedSlot(0)));

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
            {Team.Red, new Champion[]{TestHelper.CreateTraitChamp(Side.Opponent, TargetRange.Single, 999)}}
        };

        var sut = new TraitController(data);

        Assert.IsTrue(sut.UseTrait(TestHelper.CreateBlueSlot(1), TestHelper.CreateRedSlot(0)));

        // ✅ 기대: Red[0] 공격력은 11이어야 함 (시전자 Blue[1]의 효과)
        Assert.AreEqual(11, data[Team.Red][0].StatData.Attack);

        // ✅ 기대: 사용 플래그는 Blue[1]만 true
        Assert.IsFalse(sut.IsTraitUsed(TestHelper.CreateBlueSlot(0)));
        Assert.IsTrue(sut.IsTraitUsed(TestHelper.CreateBlueSlot(1)));
    }

    [Test]
    public void 조건은_실시간_반영()
    {
        Dictionary<Team, IReadOnlyList<Champion>> data = new();
        data.Add(Team.Blue, CreateTrait());
        data.Add(Team.Red, CreateTrait());
        TraitController traitPresenter = new TraitController(data);

        Assert.IsTrue(traitPresenter.UseTrait(TestHelper.CreateBlueSlot(0), TestHelper.CreateRedSlot(0)));
        Assert.AreEqual(15, data[Team.Red][0].StatData.Attack);
        Assert.AreEqual(15, data[Team.Red][1].StatData.Attack);

        // 사용은 되지만 조건이 안되서 적용 안됨
        Assert.IsTrue(traitPresenter.UseTrait(TestHelper.CreateBlueSlot(1), TestHelper.CreateRedSlot(0)));
        Assert.AreEqual(15, data[Team.Red][0].StatData.Attack);
        Assert.AreEqual(15, data[Team.Red][1].StatData.Attack);

        // 10이하면 공 15증가
        Champion[] CreateTrait() => new Champion[] { 
            TestHelper.CreateTraitChamp(Side.Opponent, TargetRange.All, 15, TraitConditionType.AttackBelow, 10),
            TestHelper.CreateTraitChamp(Side.Opponent, TargetRange.All, 15, TraitConditionType.AttackBelow, 10)
        };
    }
}
