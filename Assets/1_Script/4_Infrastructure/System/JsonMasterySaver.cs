using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public struct MasteryBoardSaveData
{
    public int Id;
    public int AttackLevel;
    public int DefenseLevel;
    public int SpeedLevel;

    public MasteryBoard CreateBoard() => new MasteryBoard(AttackLevel, DefenseLevel, SpeedLevel);
}

[Serializable]
public struct MasteryInventorySaveData
{
    public int AvailablePoints;
    public List<MasteryBoardSaveData> Boards;
}

public class JsonMasterySaver : IMasterySaver
{
    readonly string SaveKey;

    public JsonMasterySaver(string saveKey = "MasterySaveData") => SaveKey = saveKey;

    public void Save(MasteryProfile inventory)
    {
        var saveData = new MasteryInventorySaveData
        {
            AvailablePoints = inventory.AvailablePoints,
            Boards = new List<MasteryBoardSaveData>()
        };

        foreach (var kvp in inventory.BoardCollection.AllBoards)
        {
            saveData.Boards.Add(new MasteryBoardSaveData
            {
                Id = kvp.Key,
                AttackLevel = kvp.Value.AttackLevel,
                DefenseLevel = kvp.Value.DefenseLevel,
                SpeedLevel = kvp.Value.SpeedLevel
            });
        }

        string json = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
    }

    public MasteryProfile Load()
    {
        if (PlayerPrefs.HasKey(SaveKey) == false) return null;

        string json = PlayerPrefs.GetString(SaveKey);
        var saveData = JsonUtility.FromJson<MasteryInventorySaveData>(json);
        var loadedBoards = saveData.Boards.ToDictionary(x => x.Id, x => x.CreateBoard());

        return new MasteryProfile(saveData.AvailablePoints, new MasteryBoardCollection(loadedBoards));
    }
}