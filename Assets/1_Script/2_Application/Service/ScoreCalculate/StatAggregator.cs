using System.Collections.Generic;
using System.Linq;

public class StatAggregator
{
    public ChampionStatData AggregateStat(IEnumerable<ChampionStatData> stats)
    {
        int attack = stats.Sum(s => s.Attack);
        int defense = stats.Sum(s => s.Defense);

        return new ChampionStatData(attack, defense);
    }
}
