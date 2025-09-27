using NUnit.Framework;

public class StorageFactoryTests
{
    ChampionStorageFactory sut;
    SlotStorage<int> data;

    [SetUp]
    public void Setup()
    {
        data = new SlotStorage<int>();
        data.AddSlot(Team.Blue, 1);
        data.AddSlot(Team.Red, 2);
        ChampionCatalog championCatalog = new ChampionCatalog(new Champion[] { new Champion(1, "일", new ChampionStatData(10, 10, 10), default, null), new Champion(2, "이", new ChampionStatData(20, 20, 20), default, null) });
        sut = new(championCatalog);
    }

    [Test]
    public void ID를_챔피언_상태로_변환()
    {
        SlotStorage<ChampionStatus> result = sut.CreateStatusStorage(data);

        Assert.AreEqual(new ChampionStatData(10, 10, 10), result.GetSlot(TestHelper.CreateBlueSlot(0)).Stat);
        Assert.AreEqual(new ChampionStatData(20, 20, 20), result.GetSlot(TestHelper.CreateRedSlot(0)).Stat);
    }

    [Test]
    public void ID를_챔피언으로_변환()
    {
        SlotStorage<Champion> result = sut.CreateChampionStorage(data);

        Assert.AreEqual("일", result.GetSlot(TestHelper.CreateBlueSlot(0)).Name);
        Assert.AreEqual("이", result.GetSlot(TestHelper.CreateRedSlot(0)).Name);
    }
}
