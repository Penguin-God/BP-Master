using NUnit.Framework;

public class TraitTargetFindingTests
{
    TraitTargetSelector CreateSut(int count) => new TraitTargetSelector(count);

    [Test]
    public void 타겟_팀전체_슬롯_반환()
    {
        var sut = CreateSut(3);

        CollectionAssert.AreEquivalent(TestHelper.CreateRedSlots(0, 1, 2), sut.GetTargetableSlot(Team.Blue, Side.Opponent));
        CollectionAssert.AreEquivalent(TestHelper.CreateBlueSlots(0, 1, 2), sut.GetTargetableSlot(Team.Blue, Side.Self));
        CollectionAssert.AreEquivalent(TestHelper.CreateBlueSlots(0, 1, 2), sut.GetTargetableSlot(Team.Blue, Side.Self));
        CollectionAssert.AreEquivalent(TestHelper.CreateBlueSlots(0, 1, 2), sut.GetTargetableSlot(Team.Red, Side.Opponent));
    }

    [Test]
    public void 범위에_따른_타겟_슬롯들_반환()
    {
        var sut = CreateSut(3);

        CollectionAssert.AreEquivalent(TestHelper.CreateRedSlots(0, 1, 2), sut.GetTargetSlots(TargetRange.All, TestHelper.CreateRedSlot(0)));
        CollectionAssert.AreEquivalent(TestHelper.CreateBlueSlots(1), sut.GetTargetSlots(TargetRange.Single, TestHelper.CreateBlueSlot(1)));

        CollectionAssert.AreEquivalent(TestHelper.CreateBlueSlots(0, 1, 2), sut.GetTargetSlots(TargetRange.All, TestHelper.CreateBlueSlot(0)));
    }
}
