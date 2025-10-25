using NUnit.Framework;
using System.Linq;
using static TestHelper;

public class StorageFactoryTests
{
    IdStorageConverter sut;
    SlotStorage<int> data;

    [SetUp]
    public void Setup()
    {
        data = new SlotStorage<int>();
        data.AddSlot(Team.Blue, 1);
        data.AddSlot(Team.Red, 2);
        ChampionCatalog championCatalog = new ChampionCatalog(new Champion[] { CreateChamp(1, "일", new ChampionStatData(10, 10, 10)), CreateChamp(2, "이", new ChampionStatData(20, 20, 20)) });
        sut = new(championCatalog);
    }

    [Test]
    public void ID를_챔피언_상태로_변환()
    {
        SlotStorage<ChampionStatus> result = sut.IdToStatus(data);

        Assert.AreEqual(new ChampionStatData(10, 10, 10), result.GetSlot(CreateBlueSlot(0)).Stat);
        Assert.AreEqual(new ChampionStatData(20, 20, 20), result.GetSlot(CreateRedSlot(0)).Stat);
    }
}
