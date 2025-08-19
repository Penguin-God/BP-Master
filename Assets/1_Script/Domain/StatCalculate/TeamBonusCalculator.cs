using System.Collections.Generic;
using System.Linq;

public class TeamBonusCalculator
{
    readonly BonusCalculator attackBonusCalculator;
    readonly BonusCalculator defenseBonusCalculator;
    readonly BonusCalculator rangeBonusCalculator;
    readonly BonusCalculator speedBonusCalculator;

    public TeamBonusCalculator(BonusCalculator bonusCalculator1, BonusCalculator bonusCalculator2, BonusCalculator bonusCalculator3, BonusCalculator bonusCalculator4)
    {
        this.attackBonusCalculator = bonusCalculator1;
        this.defenseBonusCalculator = bonusCalculator2;
        this.rangeBonusCalculator = bonusCalculator3;
        this.speedBonusCalculator = bonusCalculator4;
    }

    public int CalculateTeamBonus(IEnumerable<Champion> team)
    {
        return attackBonusCalculator.CalculateBonus(team.Sum(x => x.Attack)) + defenseBonusCalculator.CalculateBonus(team.Sum(x => x.Defense)) 
            + rangeBonusCalculator.CalculateBonus(team.Sum(x => x.Range)) + speedBonusCalculator.CalculateBonus(team.Sum(x => x.Speed));
    }
}
