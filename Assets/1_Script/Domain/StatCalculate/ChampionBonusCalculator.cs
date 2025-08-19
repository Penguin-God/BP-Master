using System;
using System.Collections.Generic;

public class ChampionBonusCalculator
{
    SortedDictionary<int, int> attackBonusData;
    SortedDictionary<int, int> defenseBonusData;
    public ChampionBonusCalculator(SortedDictionary<int, int> attackBonusData, SortedDictionary<int, int> defenseBonusData)
    {
        this.attackBonusData = attackBonusData;
        this.defenseBonusData = defenseBonusData;
    }

    public int CalculateBonus(Champion champion)
    {
        return GetBonus(attackBonusData, champion.Attack) + GetBonus(defenseBonusData, champion.Defense);
    }

    int GetBonus(SortedDictionary<int, int> bands, int sum)
    {
        int bonus = 0;
        foreach (var data in bands) // 제일 높은 값까지 갱신
        {
            if (sum >= data.Key) bonus = data.Value;
            else break;
        }
        return bonus;
    }
}
