using NUnit.Framework;
using static TestHelper;

public class TraitTargetFindingTests
{
    TraitTargetSelector CreateSut(int count) => new TraitTargetSelector(count);

    [Test]
    public void 타겟_팀전체_슬롯_반환()
    {
        var sut = CreateSut(3);

        CollectionAssert.AreEquivalent(CreateRedSlots(0, 1, 2), sut.GetTargetableSlot(Team.Blue, Side.Opponent));
        CollectionAssert.AreEquivalent(CreateBlueSlots(0, 1, 2), sut.GetTargetableSlot(Team.Blue, Side.Self));
        CollectionAssert.AreEquivalent(CreateBlueSlots(0, 1, 2), sut.GetTargetableSlot(Team.Blue, Side.Self));
        CollectionAssert.AreEquivalent(CreateBlueSlots(0, 1, 2), sut.GetTargetableSlot(Team.Red, Side.Opponent));
    }

    [Test]
    public void 범위에_따른_타겟_슬롯들_반환()
    {
        var sut = CreateSut(3);

        CollectionAssert.AreEquivalent(CreateRedSlots(0, 1, 2), sut.GetTargetSlots(TargetRange.All, CreateRedSlot(0)));
        CollectionAssert.AreEquivalent(CreateBlueSlots(1), sut.GetTargetSlots(TargetRange.Single, CreateBlueSlot(1)));

        CollectionAssert.AreEquivalent(CreateBlueSlots(0, 1, 2), sut.GetTargetSlots(TargetRange.All, CreateBlueSlot(0)));
    }

    [Test]
    public void 사이드와_범위가_All이면_모든_타겟_반환()
    {
        var sut = CreateSut(2);

        var result = sut.GetTargetSlots(new TraitTargetRule(Side.All, TargetRange.All), CreateRedSlot(0));

        CollectionAssert.AreEquivalent(new SlotData[] { CreateBlueSlot(0), CreateBlueSlot(1), CreateRedSlot(0), CreateRedSlot(1) }, result);
    }
}
