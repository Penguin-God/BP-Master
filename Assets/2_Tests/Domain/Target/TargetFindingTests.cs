using NUnit.Framework;
using System.Collections.Generic;
using static TestHelper;

public class TraitTargetFindingTests
{
    SkillTargetFinder CreateSut(int count) => new SkillTargetFinder(count);

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
}
