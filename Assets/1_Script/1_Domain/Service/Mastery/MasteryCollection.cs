using System.Collections.Generic;
using System.Linq;

public record MasteryMultiplier(int Attack, int Defense, int Speed);

public class MasteryCollection : IMasteryStatProvider
{
    readonly Dictionary<int, ChampionMastery> masteryMap;
    public IEnumerable<ChampionMastery> AllMasteries => masteryMap.Values;

    public MasteryCollection(IEnumerable<ChampionMastery> masteries) => masteryMap = masteries.ToDictionary(m => m.ChampionId, m => m);

    public int GetMasteryLevel(int championId) => HasMastery(championId) ? masteryMap[championId].MasteryStat.Attack : 0;
    public ChampionStatData GetMasteryStat(int championId) => HasMastery(championId) ? masteryMap[championId].MasteryStat : default;
    bool HasMastery(int championId) => masteryMap.ContainsKey(championId);
}