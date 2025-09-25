using NUnit.Framework;

public class ChampionFindingTests
{
    ChampionCatalog manager;
    
    Champion CreateChamp(int id, string name) => new Champion(id, name, default, default, null);

    [SetUp]
    public void SetUp()
    {
        var testChampions = new[]
        {
            CreateChamp(1, "전사"),
            CreateChamp(2, "마법사"),
            CreateChamp(3, "암살자"),
            CreateChamp(4, "탱커"),
            CreateChamp(5, "서포터")
        };

        manager = new ChampionCatalog(testChampions);
    }

    [Test]
    public void 모든_챔피언_ID_목록()
    {
        var allIds = manager.AllId;

        Assert.That(allIds.Count, Is.EqualTo(5));
        Assert.That(allIds, Is.EquivalentTo(new[] { 1, 2, 3, 4, 5 }));
    }

    [Test]
    public void ID로_챔피언_데이터_조회()
    {
        var champion = manager.GetChampion(3);

        Assert.That(champion.Name, Is.EqualTo("암살자"));
    }

    [Test]
    public void 조회한_데이터는_깊은_복사()
    {
        var champion = manager.GetChampion(3);
        champion.ChangeStat(new ChampionStatData(10, 10, 10));

        Assert.AreEqual(0, manager.GetChampion(3).StatData.Attack);
    }
}