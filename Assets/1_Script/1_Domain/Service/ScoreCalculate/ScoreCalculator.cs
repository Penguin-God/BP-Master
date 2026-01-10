using System.Collections.Generic;
using System.Linq;

public static class ScoreCalculator
{
    public static int CalculateDefaultScore(IEnumerable<ChampionStatData> team) => CalculateAttack(team) + CalculateDefense(team);
    public static int CalculateAttack(IEnumerable<ChampionStatData> team) => team.Sum(x => x.Attack);
    public static int CalculateDefense(IEnumerable<ChampionStatData> team) => team.Sum(x => x.Defense);
    public static int CalculateSpeed(IEnumerable<ChampionStatData> team) => team.Sum(x => x.Speed);
}
