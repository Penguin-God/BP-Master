using System.Collections.Generic;
using System.Linq;

public class TeamBonusCalculator
{
    readonly BonusCalculator attackBonusCalculator;
    readonly BonusCalculator defenseBonusCalculator;
    readonly BonusCalculator speedBonusCalculator;

    public TeamBonusCalculator(BonusCalculator bonusCalculator1, BonusCalculator bonusCalculator2, BonusCalculator bonusCalculator3)
    {
        this.attackBonusCalculator = bonusCalculator1;
        this.defenseBonusCalculator = bonusCalculator2;
        this.speedBonusCalculator = bonusCalculator3;
    }

    public int CalculateTeamBonus(IEnumerable<ChampionStatData> team)
    {
        return attackBonusCalculator.CalculateBonus(team.Sum(x => x.Attack)) + defenseBonusCalculator.CalculateBonus(team.Sum(x => x.Defense)) 
            + speedBonusCalculator.CalculateBonus(team.Sum(x => x.Speed));
    }
}
