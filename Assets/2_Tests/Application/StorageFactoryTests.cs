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

    [Test]
    public void ID를_챔피언으로_변환()
    {
        SlotStorage<Champion> result = sut.IdToChampion(data);

        Assert.AreEqual("일", result.GetSlot(CreateBlueSlot(0)).Name);
        Assert.AreEqual("이", result.GetSlot(CreateRedSlot(0)).Name);
    }

    [Test]
    public void 챔피언을_특성_데이터로_반환()
    {
        var championStorage = new SlotStorage<Champion>();
        championStorage.AddSlots(Team.Blue, new[] { CreateTraitChamp(Side.All, TargetRange.All, 10), });
        championStorage.AddSlots(Team.Red, new[] { CreateTraitChamp(Side.All, TargetRange.All, 50), });

        // Act
        var result = ChampionStorageConverter.ChamptionToTrait(championStorage);

        // Assert
        Assert.AreEqual(1, result.GetTeam(Team.Blue).Count());
        Assert.AreEqual(1, result.GetTeam(Team.Red).Count());
        Assert.AreEqual(10, result.GetSlot(CreateBlueSlot(0)).ToArray()[0].Amount);
    }
}
