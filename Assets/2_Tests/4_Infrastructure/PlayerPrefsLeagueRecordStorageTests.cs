using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsLeagueRecordStorageTests
{
    const string TestKey = "TestSaveKey_Shared";

    [TearDown]
    public void TearDown()
    {
        PlayerPrefs.DeleteKey(TestKey);
    }

    [Test]
    public void 데이터를_저장하고_불러오면_동일한_값이_반환된다()
    {
        var storage = CreateStorage(TestKey);
        var collection = CreateTestCollection();

        storage.SaveAll(collection);
        var loadedCollection = storage.LoadAll();

        var originalP1 = collection.Get(1);
        var loadedP1 = loadedCollection.Get(1);

        Assert.AreEqual(originalP1.Win, loadedP1.Win);
        Assert.AreEqual(originalP1.Lose, loadedP1.Lose);
        Assert.AreEqual(originalP1.Score, loadedP1.Score);
    }

    [Test]
    public void 저장된_데이터가_없으면_빈_컬렉션을_반환한다()
    {
        var storage = CreateStorage(TestKey);

        var loadedCollection = storage.LoadAll();

        Assert.IsEmpty(loadedCollection.GetAll());
    }

    PlayerPrefsLeagueRecordStorage CreateStorage(string key) => new PlayerPrefsLeagueRecordStorage(key);

    LeagueRecordCollection CreateTestCollection()
    {
        var records = new List<LeagueRecord>
        {
            new LeagueRecord(id: 1, win: 2, lose: 1, score: 5),
            new LeagueRecord(id: 2, win: 0, lose: 3, score: -2)
        };
        return new LeagueRecordCollection(records);
    }
}