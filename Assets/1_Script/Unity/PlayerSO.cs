using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[System.Serializable]
public class ChampionMasteryData
{
    [SerializeField] ChampionSO champion;
    public ChampionSO Champion => champion;
    public int level;

    public ChampionMasteryData(ChampionSO champion, int level)
    {
        this.champion = champion;
        this.level = level;
    }

    public ChampionMasteryData GetClone() => new ChampionMasteryData(champion, level);
}

[CreateAssetMenu(fileName = "PlayerSO", menuName = "BP Master/PlayerSO")]
public class PlayerSO : ScriptableObject
{
    [SerializeField] int id;
    public int Id => id;

    [SerializeField] string playerName;
    public string PlayerName => playerName;

    [SerializeField] ChampionMasteryData[] startMasteries;
    public IReadOnlyList<ChampionMasteryData> StartMasteries => startMasteries.Select(x => x.GetClone()).ToArray();

    public int GetMasteryLevel(int chamId)
    {
        if (TryGetMastery(chamId, out var mastery))
            return mastery.level;
        else return 0;
    }

    bool TryGetMastery(int chamId, out ChampionMasteryData mastery)
    {
        mastery = null;
        if (startMasteries.Any(x => x.Champion.Id == chamId))
        {
            mastery = startMasteries.First(x => x.Champion.Id == chamId);
            return true;
        }
        else
            return false;
    }
}
