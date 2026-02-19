using System.Collections.Generic;
using System.Linq;

public class ChampionStatSorter
{
    public IEnumerable<Champion> SortByStat(IEnumerable<Champion> champions, StatType statType)
    {
        return statType switch
        {
            StatType.Attack => champions.OrderByDescending(c => c.Status.Stat.Attack).ThenBy(c => c.Id),
            StatType.Defense => champions.OrderByDescending(c => c.Status.Stat.Defense).ThenBy(c => c.Id),
            StatType.Speed => champions.OrderByDescending(c => c.Status.Stat.Speed).ThenBy(c => c.Id),
            _ => champions.OrderBy(c => c.Id)
        };
    }
}