using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

[System.Serializable]
public class LeagueRecordData
{
    public int Id;
    public int Win;
    public int Lose;
    public int Score;

    public LeagueRecordData() { }

    public LeagueRecordData(int id, int win, int lose, int score)
    {
        Id = id;
        Win = win;
        Lose = lose;
        Score = score;
    }

    public LeagueRecord ToDomain() => new LeagueRecord(Id, Win, Lose, Score);
}

public class PlayerPrefsLeagueRecordStorage : ILeagueRecordStorage
{
    readonly string saveKey;

    public PlayerPrefsLeagueRecordStorage(string saveKey = "LeagueRecords_Data")
    {
        this.saveKey = saveKey;
    }

    public LeagueRecordCollection LoadAll()
    {
        string json = PlayerPrefs.GetString(saveKey, string.Empty);

        if (string.IsNullOrEmpty(json))
            return new LeagueRecordCollection(Enumerable.Empty<LeagueRecord>());

        var dataList = JsonConvert.DeserializeObject<List<LeagueRecordData>>(json);
        var domainRecords = dataList.Select(data => data.ToDomain());

        return new LeagueRecordCollection(domainRecords);
    }

    public void SaveAll(LeagueRecordCollection collection)
    {
        var dataList = collection.GetAll().Select(r => new LeagueRecordData(r.Id, r.Win, r.Lose, r.Score)).ToList();

        string json = JsonConvert.SerializeObject(dataList);
        PlayerPrefs.SetString(saveKey, json);
        PlayerPrefs.Save();
    }
}