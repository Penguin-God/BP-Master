using NUnit.Framework;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

public class ChampionManagementTests
{
    ChampionManagerA manager;
    Champion[] testChampions;

    [SetUp]
    public void SetUp()
    {
        testChampions = new[]
        {
            new Champion(1, "전사", default),
            new Champion(2, "마법사", default),
            new Champion(3, "암살자", default),
            new Champion(4, "탱커", default),
            new Champion(5, "서포터", default)
        };

        manager = new ChampionManagerA(testChampions);
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