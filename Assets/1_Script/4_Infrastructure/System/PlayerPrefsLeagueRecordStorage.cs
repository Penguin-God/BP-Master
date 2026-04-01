using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

[System.Serializable]
public class LeagueRecordData
{
    public int Win;
    public int Lose;
    public int Score;

    public LeagueRecordData() { } // 리플랙션용

    public LeagueRecordData(int win, int lose, int score)
    {
        Win = win;
        Lose = lose;
        Score = score;
    }

    public LeagueRecord ToDomain() => new LeagueRecord(Win, Lose, Score);
}

public class PlayerPrefsLeagueRecordStorage : ILeagueRecordStorage
{
    readonly string saveKey;

    public PlayerPrefsLeagueRecordStorage(string saveKey = "LeagueRecords_Data")
    {
        this.saveKey = saveKey;
    }

    public Dictionary<int, LeagueRecord> LoadAll()
    {
        string json = PlayerPrefs.GetString(saveKey, string.Empty);

        if (string.IsNullOrEmpty(json))
            return new Dictionary<int, LeagueRecord>();

        var dataDict = JsonConvert.DeserializeObject<Dictionary<int, LeagueRecordData>>(json);
        return dataDict.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToDomain());
    }

    public void SaveAll(Dictionary<int, LeagueRecord> records)
    {
        var dataDict = records.ToDictionary(
            kvp => kvp.Key,
            kvp => new LeagueRecordData(kvp.Value.Win, kvp.Value.Lose, kvp.Value.Score)
        );

        string json = JsonConvert.SerializeObject(dataDict);
        PlayerPrefs.SetString(saveKey, json);
        PlayerPrefs.Save();
    }
}