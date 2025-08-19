using System;

public class ChampionBonusCalculator
{
    readonly BonusCalculator attackBonusCalculator;
    readonly BonusCalculator defenseBonusCalculator;
    public ChampionBonusCalculator(BonusCalculator attackBonusCalculator, BonusCalculator defenseBonusCalculator)
    {
        this.attackBonusCalculator = attackBonusCalculator;
        this.defenseBonusCalculator = defenseBonusCalculator;
    }

    public int CalculateBonus(Champion champion)
    {
        return attackBonusCalculator.CalculateBonus(champion.Attack) + defenseBonusCalculator.CalculateBonus(champion.Defense);
    }
}
