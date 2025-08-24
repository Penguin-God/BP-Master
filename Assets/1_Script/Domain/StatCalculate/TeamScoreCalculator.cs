using System.Collections.Generic;
using System.Linq;

public class TeamScoreCalculator
{
    readonly ChampionBonusCalculator championBonusCalculator;
    readonly TeamBonusCalculator teamBonusCalculator;
    public TeamScoreCalculator(ChampionBonusCalculator championBonusCalculator, TeamBonusCalculator teamBonusCalculator)
    {
        this.championBonusCalculator = championBonusCalculator;
        this.teamBonusCalculator = teamBonusCalculator;
    }

    public int CalculateScore(IEnumerable<ChampionStatData> team)
    {
        return team.Sum(x => x.Attack + x.Defense) 
            + teamBonusCalculator.CalculateTeamBonus(team) 
            + team.Sum(x => championBonusCalculator.CalculateBonus(x));
    }
}
