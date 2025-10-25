using NUnit.Framework;

public class SlotStorageTests
{
    [Test]
    public void 슬롯에_추가한_순서로_저장_및_조회()
    {
        SlotStorage<int> sut = new();

        sut.AddSlot(Team.Blue, 10);
        sut.AddSlot(Team.Blue, 20);

        Assert.AreEqual(10, sut.GetSlot(TestHelper.CreateBlueSlot(0)));
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
    public void 슬롯갑_변경()
    {
        SlotStorage<bool> sut = new();
        sut.AddSlot(Team.Blue, false);
        sut.AddSlot(Team.Blue, false);

        sut.ChangeSlot(TestHelper.CreateBlueSlot(0), true);

        Assert.AreEqual(true, sut.GetSlot(TestHelper.CreateBlueSlot(0)));
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

    [Test]
    public void 슬롯_데이터_가져오기()
    {
        SlotStorage<bool> sut = new();

        sut.AddSlots(Team.Blue, new bool[] { true, false });
        sut.AddSlots(Team.Red, new bool[] { false, true });

        CollectionAssert.AreEqual(new SlotData[] { TestHelper.CreateBlueSlot(0), TestHelper.CreateBlueSlot(1), TestHelper.CreateRedSlot(0), TestHelper.CreateRedSlot(1), }, sut.GetAllSlotDatas());
    }
}
