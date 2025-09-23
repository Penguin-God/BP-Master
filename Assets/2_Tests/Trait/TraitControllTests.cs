using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

public class TraitControllTests
{
    //[Test]
    public void 한_챔피언이_특성_중복_사용_불가()
    {
        SlotStorage<Champion> storage = new();
        storage.AddSlot(Team.Blue, TestHelper.CreateTraitChamp(Side.Opponent, TargetRange.Single, 10));
        storage.AddSlot(Team.Red, TestHelper.CreateTraitChamp(Side.Opponent, TargetRange.Single, 10));
        TraitController traitPresenter = new TraitController(storage);

        Assert.IsTrue(traitPresenter.UseTrait(TestHelper.CreateBlueSlot(0), TestHelper.CreateRedSlot(0)));
        Assert.AreEqual(10, storage.GetSlot(TestHelper.CreateRedSlot(0)).StatData.Attack);

        Assert.IsFalse(traitPresenter.UseTrait(TestHelper.CreateBlueSlot(0), TestHelper.CreateRedSlot(0)));
        Assert.AreEqual(10, storage.GetSlot(TestHelper.CreateRedSlot(0)).StatData.Attack);
    }

    [Test]
    public void 특성_시전_후_사용_플래그_true()
    {
        SlotStorage<Champion> storage = new();
        storage.AddSlot(Team.Blue, TestHelper.CreateTraitChamp(Side.Opponent, TargetRange.Single, 10));
        storage.AddSlot(Team.Red, TestHelper.CreateTraitChamp(Side.Opponent, TargetRange.Single, 10));
        var sut = new TraitController(storage);

        Assert.IsTrue(sut.UseTrait(TestHelper.CreateBlueSlot(0), TestHelper.CreateRedSlot(0)));

        // 시전자 슬롯만 true
        Assert.IsTrue(sut.IsTraitUsed(TestHelper.CreateBlueSlot(0)));
        Assert.IsFalse(sut.IsTraitUsed(TestHelper.CreateRedSlot(0)));
    }

    [Test]
    public void 시전자와_타겟_인덱스_달라질_때()
    {
        SlotStorage<Champion> storage = new();
        storage.AddSlot(Team.Blue, TestHelper.CreateTraitChamp(Side.Opponent, TargetRange.Single, 5));
        storage.AddSlot(Team.Blue, TestHelper.CreateTraitChamp(Side.Opponent, TargetRange.Single, 11));

        storage.AddSlot(Team.Red, TestHelper.CreateTraitChamp(Side.Opponent, TargetRange.Single, 999));

        var sut = new TraitController(storage);

        Assert.IsTrue(sut.UseTrait(TestHelper.CreateBlueSlot(1), TestHelper.CreateRedSlot(0)));

        // ✅ 기대: Red[0] 공격력은 11이어야 함 (시전자 Blue[1]의 효과)
        Assert.AreEqual(11, storage.GetSlot(TestHelper.CreateRedSlot(0)).StatData.Attack);

        // ✅ 기대: 사용 플래그는 Blue[1]만 true
        Assert.IsFalse(sut.IsTraitUsed(TestHelper.CreateBlueSlot(0)));
        Assert.IsTrue(sut.IsTraitUsed(TestHelper.CreateBlueSlot(1)));
    }

    [Test]
    public void 조건은_실시간_반영()
    {
        SlotStorage<Champion> storage = new();
        storage.AddSlots(Team.Blue, CreateTrait());
        storage.AddSlots(Team.Red, CreateTrait());

        TraitController sut = new TraitController(storage);

        Assert.IsTrue(sut.UseTrait(TestHelper.CreateBlueSlot(0), TestHelper.CreateRedSlot(0)));
        Assert.AreEqual(15, storage.GetSlot(TestHelper.CreateRedSlot(0)).StatData.Attack);
        Assert.AreEqual(15, storage.GetSlot(TestHelper.CreateRedSlot(1)).StatData.Attack);

        // 사용은 되지만 조건이 안되서 적용 안됨
        Assert.IsTrue(sut.UseTrait(TestHelper.CreateBlueSlot(1), TestHelper.CreateRedSlot(0)));
        Assert.AreEqual(15, storage.GetSlot(TestHelper.CreateRedSlot(0)).StatData.Attack);
        Assert.AreEqual(15, storage.GetSlot(TestHelper.CreateRedSlot(1)).StatData.Attack);

        // 10이하면 공 15증가
        Champion[] CreateTrait() => new Champion[] {
            TestHelper.CreateTraitChamp(Side.Opponent, TargetRange.All, 15, TraitConditionType.AttackBelow, 10),
            TestHelper.CreateTraitChamp(Side.Opponent, TargetRange.All, 15, TraitConditionType.AttackBelow, 10)
        };
    }

    [Test]
    public void 특성_적용시_피드백_여러_타겟()
    {
        // Arrange
        SlotStorage<Champion> storage = new();
        storage.AddSlot(Team.Blue, TestHelper.CreateTraitChamp(Side.Opponent, TargetRange.All, 10));
        storage.AddSlot(Team.Blue, TestHelper.CreateTraitChamp(Side.Opponent, TargetRange.All, 10));

        storage.AddSlot(Team.Red, TestHelper.CreateStatChamp(0));
        storage.AddSlot(Team.Red, TestHelper.CreateStatChamp(0));

        var sut = new TraitController(storage);

        List<StatChangeData> lastFeedback = new List<StatChangeData>();
        sut.OnTraitApplied += fb => lastFeedback.Add(fb);

        // Act
        sut.UseTrait(TestHelper.CreateBlueSlot(0), TestHelper.CreateRedSlot(0));

        // Assert - 피드백이 2개(두 타겟)에 대해 왔는지
        Assert.AreEqual(2, lastFeedback.Count);

        // 어떤 슬롯들이 대상이었는지 (순서 무관 검증)
        var receivedSlots = lastFeedback.Select(f => f.Slot).ToArray();
        CollectionAssert.AreEquivalent(new[] { TestHelper.CreateRedSlot(0), TestHelper.CreateRedSlot(1) }, receivedSlots);

        Assert.AreEqual(0, lastFeedback[0].Before.Attack);
        Assert.AreEqual(10, lastFeedback[0].After.Attack);

        Assert.AreEqual(0, lastFeedback[1].Before.Attack);
        Assert.AreEqual(10, lastFeedback[1].After.Attack);
    }
}
