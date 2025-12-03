using NUnit.Framework;
using System.Linq;
using static TestHelper;

public class TraitTargetFilteringTests
{
    SkillSlotFilter sut;
    SlotStorage<bool> flags;
    [SetUp]
    public void SetUp()
    {
        flags = new SlotStorage<bool>();
        flags.AddSlots(Team.Red, new bool[] { false, false });
        flags.AddSlots(Team.Blue, new bool[] { false, false });
        sut = new SkillSlotFilter(flags);
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


    SkillTargetFilter CreateSut(int blueCount = 3, int redCount = 2) => new SkillTargetFilter(new TeamCounter(blueCount, redCount));
    [Test]
    public void Self_사이드면_자기팀_슬롯만_반환()
    {
        var sut = CreateSut(3, 2);

        var blueSlots = sut.FilteringTargetSlots(Team.Blue, new[] { Side.Self }).ToArray();
        Assert.AreEqual(3, blueSlots.Count());
    }

    [Test]
    public void Opponent_사이드면_상대팀_슬롯만_반환()
    {
        var sut = CreateSut(3, 2);

        var fromBlue = sut.FilteringTargetSlots(Team.Blue, new[] { Side.Opponent }).ToArray();

        Assert.AreEqual(2, fromBlue.Count());
    }

    [Test]
    public void Self와_Opponent가_섞이면_All로_머지되어_양팀_슬롯_모두_반환()
    {
        var sut = CreateSut(3, 2);

        var slots = sut.FilteringTargetSlots(Team.Blue, new[] { Side.Self, Side.Opponent });

        Assert.AreEqual(5, slots.Count());
        Assert.AreEqual(3, slots.Count(x => x.Team == Team.Blue));
        Assert.AreEqual(2, slots.Count(x => x.Team == Team.Red));
    }
}
