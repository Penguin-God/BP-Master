using NUnit.Framework;

public class TraitUsePresenterTests
{
    SlotData CreateSlot(Team team, int index) => new SlotData(team, index);

    [Test]
    public void 슬롯_선택시_저장()
    {
        // Arrange
        var sut = new SlotSelectionState();
        var slot = CreateSlot(Team.Blue, 2);

        // Act
        sut.SelectSlot(slot);

        // Assert
        Assert.IsTrue(sut.IsSelect);
        Assert.AreEqual(slot, sut.SelectedSlot);
    }

    [Test]
    public void Use_호출시_상태_초기화()
    {
        // Arrange
        var sut = new SlotSelectionState();
        var slot = CreateSlot(Team.Red, 1);
        sut.SelectSlot(slot);

        // Act
        sut.Use();

        // Assert
        Assert.IsFalse(sut.IsSelect, "Use 후 IsSelect는 false여야 합니다.");
    }

    [Test]
    public void 여러번_선택_마지막으로_선택된_슬롯_저장()
    {
        // Arrange
        var sut = new SlotSelectionState();

        var first = CreateSlot(Team.Blue, 0);
        var second = CreateSlot(Team.Red, 3);

        // Act
        sut.SelectSlot(first);
        sut.SelectSlot(second);

        // Assert
        Assert.AreEqual(second, sut.SelectedSlot);
    }
}