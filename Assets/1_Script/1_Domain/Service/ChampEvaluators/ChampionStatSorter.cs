using System.Collections.Generic;
using System.Linq;

public class ChampionStatSorter
{
    public IEnumerable<Champion> SortByStat(IEnumerable<Champion> champions, StatType statType)
        => champions
            .OrderByDescending(c => c.Status.Stat.GetStatValue(statType))
            .ThenBy(c => c.Id);
}