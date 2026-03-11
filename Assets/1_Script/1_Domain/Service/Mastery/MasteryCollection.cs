using System.Collections.Generic;
using System.Linq;

public class MasteryCollection
{
    readonly Dictionary<int, ChampionMastery> masteryMap;
    public IEnumerable<ChampionMastery> AllMasteries => masteryMap.Values;

    public MasteryCollection(IEnumerable<ChampionMastery> masteries)
    {
        masteryMap = masteries.ToDictionary(m => m.ChampionId, m => m);
    }

    public int GetMasteryLevel(int championId) => HasMastery(championId) ? masteryMap[championId].Level : 0;
    public ChampionStatData GetMasteryStat(int championId) => HasMastery(championId) ? masteryMap[championId].MasteryStat : new ChampionStatData(0, 0, 0);
    public bool HasMastery(int championId) => masteryMap.ContainsKey(championId);

    public void AddMastery(int champId)
    {
        if (masteryMap.TryGetValue(champId, out var existing))
            masteryMap[champId] = new ChampionMastery(champId, existing.Level + 1);
        else
            masteryMap.Add(champId, new ChampionMastery(champId, 1));
    }
}