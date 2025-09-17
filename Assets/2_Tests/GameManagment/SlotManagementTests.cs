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

    [Test]
    public void 팀별_조회()
    {
        SlotStorage sut = new();

        var champ1 = TestHelper.CreateStatChamp(10);
        var champ2 = TestHelper.CreateStatChamp(20);
        sut.AddSlot(Team.Blue, champ1);
        sut.AddSlot(Team.Blue, champ2);

        CollectionAssert.AreEqual(new Champion[] { champ1, champ2 }, sut.GetTeam(Team.Blue));
    }
}
