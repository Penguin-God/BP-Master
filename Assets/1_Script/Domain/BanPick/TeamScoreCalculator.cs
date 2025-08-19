using System.Collections.Generic;
using System.Linq;

public class TeamScoreCalculator
{
    readonly Dictionary<int, int> bonusData;

    public TeamScoreCalculator(Dictionary<int, int> bonusData)
    {
        this.bonusData = bonusData;
    }

    public int CalculateScore(IEnumerable<Champion> team)
    {

        return team.Sum(x => x.Attack + x.Defense);
    }
}
