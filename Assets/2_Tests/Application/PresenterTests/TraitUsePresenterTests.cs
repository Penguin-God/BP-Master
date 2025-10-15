using NUnit.Framework;

public class TraitUsePresenterTests
{
    TraitUsePresenter sut;

    [SetUp]
    public void SetUp()
    {
        sut = new TraitUsePresenter(Team.Blue);
    }

    [Test]
    public void 특성_사용_데이터_반환()
    {
        TraitUseSlotData result;
        bool useable = sut.ClickChampion(CreateSlot(Team.Blue, 0), out result);
        Assert.IsFalse(useable);
        useable = sut.ClickChampion(CreateSlot(Team.Red, 0), out result);

        Assert.IsTrue(useable);
        Assert.AreEqual(CreateSlot(Team.Blue, 0), result.UseSlot);
        Assert.AreEqual(CreateSlot(Team.Red, 0), result.TargetSlot);
    }

    [Test]
    public void 선택에_따라_결과_반환()
    {
        TraitSelectionState sut = new(Team.Blue);
        SlotData useSlot = CreateSlot(Team.Blue, 0);

        SlotData? result = sut.ClickTraitSlot(CreateSlot(Team.Red, 0));
        Assert.AreEqual(null, result);

        result = sut.ClickTraitSlot(useSlot);
        Assert.AreEqual(null, result);

        result = sut.ClickTraitSlot(CreateSlot(Team.Red, 0));
        Assert.AreEqual(useSlot, result.Value);
    }

    SlotData CreateSlot(Team team, int index) => new SlotData(team, index);
}