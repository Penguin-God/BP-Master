using NUnit.Framework;

public class SlotManagementTests
{
    [Test]
    public void 슬롯에_추가한_순서로_저장_및_조회()
    {
        SlotStorage<Champion> sut = new();

        sut.AddSlot(Team.Blue, TestHelper.CreateStatChamp(10));
        sut.AddSlot(Team.Blue, TestHelper.CreateStatChamp(20));

        Assert.AreEqual(10, sut.GetSlot(TestHelper.CreateBlueSlot(0)).StatData.Attack);
    }

    [Test]
    public void 팀별_조회()
    {
        SlotStorage<bool> sut = new();

        sut.AddSlot(Team.Blue, false);
        sut.AddSlot(Team.Blue, true);

        CollectionAssert.AreEqual(new bool[] { false, true }, sut.GetTeam(Team.Blue));
    }

    [Test]
    public void 생성자에_슬롯_수_넣으면_인자값으로_초기화()
    {
        SlotStorage<bool> sut = new(3, false);

        CollectionAssert.AreEqual(new bool[] { false, false, false }, sut.GetTeam(Team.Blue));
        CollectionAssert.AreEqual(new bool[] { false, false, false }, sut.GetTeam(Team.Red));
    }

    [Test]
    public void 슬롯갑_변경()
    {
        SlotStorage<bool> sut = new(3, false);

        sut.ChangeSlot(TestHelper.CreateBlueSlot(1), true);

        CollectionAssert.AreEqual(new bool[] { false, true, false }, sut.GetTeam(Team.Blue));
    }

    [Test]
    public void 컬랙션_추가()
    {
        SlotStorage<bool> sut = new();

        sut.AddSlots(Team.Blue, new bool[] { true, false, true});

        CollectionAssert.AreEqual(new bool[] { true, false, true }, sut.GetTeam(Team.Blue));
    }

    [Test]
    public void 전부_가져오기()
    {
        SlotStorage<bool> sut = new();

        sut.AddSlots(Team.Blue, new bool[] { true, false });
        sut.AddSlots(Team.Blue, new bool[] { false, true });

        CollectionAssert.AreEqual(new bool[] { true, false, false, true }, sut.GetAll());
    }
}
