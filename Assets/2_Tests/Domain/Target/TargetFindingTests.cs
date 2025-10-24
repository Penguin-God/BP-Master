using NUnit.Framework;
using System.Collections.Generic;
using static TestHelper;

public class TraitTargetFindingTests
{
    TraitTargetFinder CreateSut(int count) => new TraitTargetFinder(count);

    [Test]
    public void 타겟_팀전체_슬롯_반환()
    {
        var sut = CreateSut(2);

        CollectionAssert.AreEquivalent(CreateRedSlots(0, 1), GetSlots(Team.Blue, Side.Opponent));
        CollectionAssert.AreEquivalent(CreateBlueSlots(0, 1), GetSlots(Team.Blue, Side.Self));
        CollectionAssert.AreEquivalent(CreateBlueSlots(0, 1), GetSlots(Team.Blue, Side.Self));
        CollectionAssert.AreEquivalent(CreateBlueSlots(0, 1), GetSlots(Team.Red, Side.Opponent));
        CollectionAssert.AreEquivalent(new SlotData[] { CreateBlueSlot(0), CreateBlueSlot(1), CreateRedSlot(0), CreateRedSlot(1) }, GetSlots(Team.Red, Side.All));

        IEnumerable<SlotData> GetSlots(Team team, Side side) => sut.GetTargetableSlot(team, side);
    }

    [Test]
    public void 범위에_따른_타겟_슬롯들_반환()
    {
        var sut = CreateSut(3);

        var result = sut.GetTargetSlots(new TraitTargetRule(Side.Opponent, TargetRange.All), CreateRedSlot(0));
        CollectionAssert.AreEquivalent(CreateRedSlots(0, 1, 2), result);

        result = sut.GetTargetSlots(new TraitTargetRule(Side.Opponent, TargetRange.Single), CreateRedSlot(1));
        CollectionAssert.AreEquivalent(CreateRedSlots(1), result);
    }

    [Test]
    public void 사이드와_범위가_All이면_모든_타겟_반환()
    {
        var sut = CreateSut(2);

        var result = sut.GetTargetSlots(new TraitTargetRule(Side.All, TargetRange.All), CreateRedSlot(0));

        CollectionAssert.AreEquivalent(new SlotData[] { CreateBlueSlot(0), CreateBlueSlot(1), CreateRedSlot(0), CreateRedSlot(1) }, result);
    }
}
