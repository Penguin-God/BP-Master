using System.Collections.Generic;
using System.Linq;

public class TeamBonusCalculator
{
    readonly BonusCalculator attackBonusCalculator;
    readonly BonusCalculator defenseBonusCalculator;

    public TeamBonusCalculator(BonusCalculator bonusCalculator1, BonusCalculator bonusCalculator2)
    {
        this.attackBonusCalculator = bonusCalculator1;
        this.defenseBonusCalculator = bonusCalculator2;
    }

    public int CalculateAttackBonus(IEnumerable<ChampionStatData> team) => attackBonusCalculator.CalculateBonus(team.Sum(x => x.Attack));
    public int CalculateDefenseBonus(IEnumerable<ChampionStatData> team) => defenseBonusCalculator.CalculateBonus(team.Sum(x => x.Defense));

    public int CalculateTeamBonus(IEnumerable<ChampionStatData> team) => CalculateAttackBonus(team) + CalculateDefenseBonus(team);

    public int CalculateTeamBonus(ScoreInfo scoreInfo) => attackBonusCalculator.CalculateBonus(scoreInfo.Att) + defenseBonusCalculator.CalculateBonus(scoreInfo.Def);
}
