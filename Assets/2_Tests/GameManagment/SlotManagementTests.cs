using NUnit.Framework;

public class SlotManagementTests
{
    [Test]
    public void 슬롯에_챔피언_저장_및_조회()
    {
        SlotStorage sut = new();

        sut.AddSlot(TestHelper.CreateBlueSlot(0), TestHelper.CreateStatChamp(10));

        Assert.AreEqual(10, sut.GetSlot(TestHelper.CreateBlueSlot(0)).StatData.Attack);
    }
}
