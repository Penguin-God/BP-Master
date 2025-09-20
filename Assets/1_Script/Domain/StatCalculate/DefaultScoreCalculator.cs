using System.Collections.Generic;
using System.Linq;

public class DefaultScoreCalculator
{
    public int CalculateDefaultScore(IEnumerable<ChampionStatData> team) => CalculateAttack(team) + CalculateDefense(team);
    public int CalculateAttack(IEnumerable<ChampionStatData> team) => team.Sum(x => x.Attack);
    public int CalculateDefense(IEnumerable<ChampionStatData> team) => team.Sum(x => x.Defense);
}

// ChampionBonus 쓰는 클래스
//public class TeamScoreCalculator
//{
//    readonly ChampionBonusCalculator championBonusCalculator;
//    readonly TeamBonusCalculator teamBonusCalculator;
//    public TeamScoreCalculator(ChampionBonusCalculator championBonusCalculator, TeamBonusCalculator teamBonusCalculator)
//    {
//        this.championBonusCalculator = championBonusCalculator;
//        this.teamBonusCalculator = teamBonusCalculator;
//    }

//    public int CalculateScore(IEnumerable<ChampionStatData> team)
//    {
//        return team.Sum(x => x.Attack + x.Defense) 
//            + teamBonusCalculator.CalculateTeamBonus(team) 
//            + team.Sum(x => championBonusCalculator.CalculateBonus(x));
//    }
//}
