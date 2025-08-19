using System.Collections.Generic;
using System.Linq;

public class TeamScoreCalculator
{
    public int CalculateScore(IEnumerable<Champion> team)
    {
        return team.Sum(x => x.Attack + x.Defense);
    }
}
