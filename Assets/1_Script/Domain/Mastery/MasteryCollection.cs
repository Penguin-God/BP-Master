using System.Collections.Generic;
using System.Linq;

public class MasteryCollection
{
    readonly Dictionary<int, int> masteryMap;
    public IEnumerable<ChampionMastery> AllMasteries => masteryMap.Select(x => new ChampionMastery(x.Key, x.Value));

    public MasteryCollection(IEnumerable<ChampionMastery> masteries)
    {
        masteryMap = masteries.ToDictionary(m => m.ChampionId, m => m.Level);
    }

    public int GetMasteryLevel(int championId) => HasMastery(championId) ? masteryMap[championId] : 0;
    public bool HasMastery(int championId) => masteryMap.ContainsKey(championId);

    public void AddMastery(int champId)
    {
        if (masteryMap.ContainsKey(champId)) masteryMap[champId]++;
        else masteryMap.Add(champId, 1);
    }
}
