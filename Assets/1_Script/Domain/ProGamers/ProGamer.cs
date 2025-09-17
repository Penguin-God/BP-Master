using System.Collections.Generic;
using System.Linq;

public class ProGamer
{
    readonly Dictionary<int, int> masteryMap;

    public ProGamer(IEnumerable<ChampionMastery> masteries)
    {
        masteryMap = masteries.ToDictionary(m => m.ChampionId, m => m.Level);
    }

    public int GetMastery(int championId) => masteryMap.TryGetValue(championId, out int level) ? level : 0;
}
