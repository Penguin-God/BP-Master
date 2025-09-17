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

    public ChampionMastery GetMastery() => new ChampionMastery(champion.Id, level);
}

[CreateAssetMenu(fileName = "PlayerSO", menuName = "BP Master/PlayerSO")]
public class PlayerSO : ScriptableObject
{
    [SerializeField] int id;
    public int Id => id;

    [SerializeField] string playerName;
    public string PlayerName => playerName;

    [SerializeField] ChampionMasteryData[] startMasteries;
    public ProGamer CreateGamer() => new ProGamer(startMasteries.Select(x => x.GetMastery()));
}
