using System.Collections.Generic;
using System.Linq;

public class TeamScoreCalculator
{
    readonly StatScoreCalculator statScoreCalculator;
    public TeamScoreCalculator(StatScoreCalculator statScoreCalculator)
    {
        this.statScoreCalculator = statScoreCalculator;
    }

    public int CalculateScore(IEnumerable<Champion> team)
    {
        int defualtStat = team.Sum(x => x.Attack + x.Defense);
        return defualtStat + statScoreCalculator.CalculateScore(team.Sum(x => x.Attack), team.Sum(x => x.Defense), team.Sum(x => x.Range), team.Sum(x => x.Speed));
    }
}
