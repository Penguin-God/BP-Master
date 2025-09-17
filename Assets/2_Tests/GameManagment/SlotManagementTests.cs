using NUnit.Framework;

public class SlotManagementTests
{
    [Test]
    public void 슬롯에_추가한_순서로_저장_및_조회()
    {
        SlotStorage sut = new();

        sut.AddSlot(Team.Blue, TestHelper.CreateStatChamp(10));
        sut.AddSlot(Team.Blue, TestHelper.CreateStatChamp(20));

        Assert.AreEqual(10, sut.GetSlot(TestHelper.CreateBlueSlot(0)).StatData.Attack);
    }
}
