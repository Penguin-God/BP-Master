using NUnit.Framework;
using static TestHelper;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class TraitTargetFilteringTests
{
    SlotStorage<ChampionStatus> statuses;
    TraitSlotFilter sut;
    SlotStorage<TraitApplier> applilers;
    SlotStorage<bool> flags;
    TraitApplier CreatAppliler(SlotData slot) => new TraitApplier(statuses, slot);
    [SetUp]
    public void SetUp()
    {
        statuses = new();
        statuses.AddSlot(Team.Blue, CreateStatus());
        statuses.AddSlot(Team.Blue, CreateStatus());
        
        statuses.AddSlot(Team.Red, CreateStatus());
        statuses.AddSlot(Team.Red, CreateStatus());

        flags = new SlotStorage<bool>();
        flags.AddSlots(Team.Red, new bool[] { false, false });
        flags.AddSlots(Team.Blue, new bool[] { false, false });
        sut = new TraitSlotFilter(flags);
    }

    [Test]
    public void 특성_사용_가능한_슬롯들_필터링()
    {
        flags.ChangeSlot(BlueOneSlot, true);

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
