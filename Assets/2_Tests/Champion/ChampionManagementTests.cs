using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;

public class ChampionManagementTests
{
    ChampionCatalog manager;
    Champion[] testChampions;

    Champion CreateChamp(int id, string name) => new Champion(id, name, default, default, null);

    [SetUp]
    public void SetUp()
    {
        testChampions = new[]
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
}