using System.Collections.Generic;
using System.Linq;

public class DefaultScoreCalculator
{
    public int CalculateDefaultScore(IEnumerable<ChampionStatData> team) => CalculateAttack(team) + CalculateDefense(team);
    public int CalculateAttack(IEnumerable<ChampionStatData> team) => team.Sum(x => x.Attack);
    public int CalculateDefense(IEnumerable<ChampionStatData> team) => team.Sum(x => x.Defense);
}
