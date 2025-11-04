using System.Collections.Generic;
using System.Linq;


public class StatTeamPredictor
{
    
}


public class ChampionStatAverager
{
    readonly IEnumerable<ChampionStatData> stats;

    public ChampionStatAverager(ChampionStatData[] stats) => this.stats = stats;

    public ChampionStatData GetStatAverage()
    {
        int totalAttack = stats.Sum(s => s.Attack);
        int totalDefense = stats.Sum(s => s.Defense);

        int attack = totalAttack / stats.Count();
        int defense = totalDefense / stats.Count();

        return new ChampionStatData(attack, defense, 0);
    }
}