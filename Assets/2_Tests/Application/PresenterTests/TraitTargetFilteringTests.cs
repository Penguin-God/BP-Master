using static TestHelper;
using NUnit.Framework;

public class TraitTargetFilteringTests
{
    SlotStorage<ChampionStatus> statuses;
    TraitSlotFilter sut;
    SlotStorage<TraitApplier> applilers;
    TraitApplier CreatAppliler() => new TraitApplier(statuses);
    [SetUp]
    public void SetUp()
    {
        statuses = new();
        statuses.AddSlot(Team.Blue, CreateStatus());
        statuses.AddSlot(Team.Blue, CreateStatus());
        
        statuses.AddSlot(Team.Red, CreateStatus());
        statuses.AddSlot(Team.Red, CreateStatus());

        applilers = new();
        applilers.AddSlot(Team.Blue, CreatAppliler());
        applilers.AddSlot(Team.Blue, CreatAppliler());
        applilers.AddSlot(Team.Red, CreatAppliler());
        applilers.AddSlot(Team.Red, CreatAppliler());

        sut = new TraitSlotFilter(applilers);
    }

    [Test]
    public void 특성_사용_가능한_슬롯들_필터링()
    {
        applilers.GetSlot(CreateBlueSlot(1)).Execute(CreateTraitData(type: TraitType.AttackChanger, side:Side.Opponent), CreateRedSlot(0), CreateBlueSlot(0));

        var result = sut.FilteringUseableSlots(Team.Blue);

        CollectionAssert.AreEquivalent(CreateBlueSlots(0), result);
    }

    [Test]
    public void 특성_타겟_슬롯들_필터링()
    {
        var result = sut.FilteringTargetSlots(Team.Blue, new Side[] { Side.Opponent });
        CollectionAssert.AreEquivalent(CreateRedSlots(0, 1), result);

        result = sut.FilteringTargetSlots(Team.Blue, new Side[] { Side.Opponent, Side.Self });
        CollectionAssert.AreEquivalent(new SlotData[] { CreateBlueSlot(0), CreateBlueSlot(1), CreateRedSlot(0), CreateRedSlot(1) }, result);
    }

    [Test]
    public void 선택_특성_필터링()
    {
        var result = sut.GetSlots(true, Team.Blue, new Side[] { Side.Opponent });
        CollectionAssert.AreEquivalent(CreateRedSlots(0, 1), result);

        result = sut.GetSlots(false, Team.Blue, null);
        CollectionAssert.AreEquivalent(CreateBlueSlots(0, 1), result);
    }
}
