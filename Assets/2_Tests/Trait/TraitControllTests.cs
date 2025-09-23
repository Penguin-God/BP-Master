using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using static TestHelper;

public class TraitControllTests
{
    [Test]
    public void 한_챔피언이_특성_중복_사용_불가()
    {
        SlotStorage<ChampionStatus> statuses = new();
        statuses.AddSlot(Team.Blue, CreateStatus(0));
        statuses.AddSlot(Team.Red, CreateStatus(0));

        var sut = new TraitController(statuses);

        Assert.IsTrue(sut.UseTrait(CreateBlueSlot(0), CreateRedSlot(0), CreateTraitData(TraitType.AttackChanger, 10), TargetRange.Single));
        Assert.AreEqual(10, statuses.GetSlot(CreateRedSlot(0)).StatData.Attack);

        Assert.IsFalse(sut.UseTrait(CreateBlueSlot(0), CreateRedSlot(0), null, TargetRange.All));
        Assert.AreEqual(10, statuses.GetSlot(CreateRedSlot(0)).StatData.Attack);
    }

    [Test]
    public void 특성_시전_후_사용_플래그_true()
    {
        SlotStorage<ChampionStatus> statuses = new();
        statuses.AddSlot(Team.Blue, CreateStatus());
        statuses.AddSlot(Team.Red, CreateStatus());

        var sut = new TraitController(statuses);

        Assert.IsTrue(sut.UseTrait(CreateBlueSlot(0), CreateRedSlot(0), CreateTraitData(TraitType.AttackChanger, 10), TargetRange.Single));

        // 시전자 슬롯만 true
        Assert.IsTrue(sut.IsTraitUsed(CreateBlueSlot(0)));
        Assert.IsFalse(sut.IsTraitUsed(CreateRedSlot(0)));
    }

    [Test]
    public void 시전자와_타겟_인덱스_달라질_때()
    {
        SlotStorage<ChampionStatus> statuses = new();
        statuses.AddSlot(Team.Blue, CreateStatus());
        statuses.AddSlot(Team.Blue, CreateStatus());
        statuses.AddSlot(Team.Red, CreateStatus(0));

        var sut = new TraitController(statuses);

        Assert.IsTrue(sut.UseTrait(CreateBlueSlot(1), CreateRedSlot(0), CreateTraitData(TraitType.AttackChanger, 11), TargetRange.Single));

        // ✅ 기대: Red[0] 공격력은 11이어야 함 (시전자 Blue[1]의 효과)
        Assert.AreEqual(11, statuses.GetSlot(CreateRedSlot(0)).StatData.Attack);

        // ✅ 기대: 사용 플래그는 Blue[1]만 true
        Assert.IsFalse(sut.IsTraitUsed(CreateBlueSlot(0)));
        Assert.IsTrue(sut.IsTraitUsed(CreateBlueSlot(1)));
    }

    [Test]
    public void 조건은_실시간_반영()
    {
        SlotStorage<ChampionStatus> statuses = new();
        // Blue 2, Red 2 상태 초기화 (공격력 0)
        statuses.AddSlot(Team.Blue, CreateStatus(0));
        statuses.AddSlot(Team.Blue, CreateStatus(0));
        statuses.AddSlot(Team.Red, CreateStatus(0));
        statuses.AddSlot(Team.Red, CreateStatus(0));

        TraitController sut = new TraitController(statuses);

        Assert.IsTrue(sut.UseTrait(CreateBlueSlot(0), CreateRedSlot(0), CreateTraitData(TraitType.AttackChanger, 15, TraitConditionType.AttackBelow, 10), TargetRange.All));
        Assert.AreEqual(15, statuses.GetSlot(CreateRedSlot(0)).StatData.Attack);
        Assert.AreEqual(15, statuses.GetSlot(CreateRedSlot(1)).StatData.Attack);

        // 사용은 되지만 조건이 안되서 적용 안됨
        Assert.IsTrue(sut.UseTrait(CreateBlueSlot(1), CreateRedSlot(0), CreateTraitData(TraitType.AttackChanger, 15, TraitConditionType.AttackBelow, 10), TargetRange.All));
        Assert.AreEqual(15, statuses.GetSlot(CreateRedSlot(0)).StatData.Attack);
        Assert.AreEqual(15, statuses.GetSlot(CreateRedSlot(1)).StatData.Attack);
    }

    [Test]
    public void 특성_적용시_피드백_여러_타겟()
    {
        SlotStorage<ChampionStatus> statuses = new();
        statuses.AddSlot(Team.Blue, CreateStatus(0));
        statuses.AddSlot(Team.Blue, CreateStatus(0));
        statuses.AddSlot(Team.Red, CreateStatus(0));
        statuses.AddSlot(Team.Red, CreateStatus(0));

        var sut = new TraitController(statuses);

        List<StatChangeData> lastFeedback = new List<StatChangeData>();
        sut.OnTraitApplied += fb => lastFeedback.Add(fb);

        // Act
        sut.UseTrait(CreateBlueSlot(0), CreateRedSlot(0), CreateTraitData(TraitType.AttackChanger, 10), TargetRange.All);

        // Assert - 피드백이 2개(두 타겟)에 대해 왔는지
        Assert.AreEqual(2, lastFeedback.Count);

        // 어떤 슬롯들이 대상이었는지 (순서 무관 검증)
        var receivedSlots = lastFeedback.Select(f => f.Slot).ToArray();
        CollectionAssert.AreEquivalent(new[] { CreateRedSlot(0), CreateRedSlot(1) }, receivedSlots);

        Assert.AreEqual(0, lastFeedback[0].Before.Attack);
        Assert.AreEqual(10, lastFeedback[0].After.Attack);

        Assert.AreEqual(0, lastFeedback[1].Before.Attack);
        Assert.AreEqual(10, lastFeedback[1].After.Attack);
    }
}
