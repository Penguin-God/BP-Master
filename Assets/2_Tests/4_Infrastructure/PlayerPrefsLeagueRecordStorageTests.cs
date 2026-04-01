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
        var records = CreateTestRecords();

        storage.SaveAll(records);
        var loadedRecords = storage.LoadAll();

        Assert.AreEqual(records[1].Win, loadedRecords[1].Win);
        Assert.AreEqual(records[1].Lose, loadedRecords[1].Lose);
        Assert.AreEqual(records[1].Score, loadedRecords[1].Score);
    }

    [Test]
    public void 저장된_데이터가_없으면_빈_딕셔너리를_반환한다()
    {
        var storage = CreateStorage(TestKey);

        var loadedRecords = storage.LoadAll();

        Assert.AreEqual(0, loadedRecords.Count);
    }

    PlayerPrefsLeagueRecordStorage CreateStorage(string key) => new PlayerPrefsLeagueRecordStorage(key);

    Dictionary<int, LeagueRecord> CreateTestRecords()
    {
        return new Dictionary<int, LeagueRecord>
        {
            { 1, new LeagueRecord(2, 1, 5) },
            { 2, new LeagueRecord(0, 3, -2) }
        };
    }
}