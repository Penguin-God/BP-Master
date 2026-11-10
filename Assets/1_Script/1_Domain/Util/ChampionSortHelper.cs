using System;
using System.Collections.Generic;
using System.Linq;

public enum ChampionSortType
{
    Default,
    Attack,
    Defense
}

public static class ChampionSortHelper
{
    public static IEnumerable<int> GetTabIds(IEnumerable<Champion> allChampions, ChampionSortType sortType, IEnumerable<int> defaultIds = null) => sortType switch
    {
        ChampionSortType.Default => defaultIds ?? allChampions.Select(x => x.Id),
        ChampionSortType.Attack => SortByStat(allChampions, StatType.Attack),
        ChampionSortType.Defense => SortByStat(allChampions, StatType.Defense),
        _ => throw new NotImplementedException(),
    };

    public static IEnumerable<int> SortByStat(IEnumerable<Champion> champions, StatType statType)
        => champions
            .OrderByDescending(c => c.Status.Stat.GetStatValue(statType))
            .Select(x => x.Id);
}
