using System.Collections.Generic;

public class StatScoreCalculator
{
    readonly SortedDictionary<int, int> attackBonusData;
    readonly SortedDictionary<int, int> defenseBonusData;
    readonly SortedDictionary<int, int> rangeBonusData;
    readonly SortedDictionary<int, int> speedBonusData;

    public StatScoreCalculator(SortedDictionary<int, int> attackBonusData, SortedDictionary<int, int> defenseBonusData, SortedDictionary<int, int> rangeBonusData, SortedDictionary<int, int> speedBonusData)
    {
        this.attackBonusData = attackBonusData;
        this.defenseBonusData = defenseBonusData;
        this.rangeBonusData = rangeBonusData;
        this.speedBonusData = speedBonusData;
    }

    public int CalculateScore(int attack, int defense, int ragne, int speed)
    {
        return attack + defense + GetBonus(attackBonusData, attack) + GetBonus(defenseBonusData, defense) + GetBonus(rangeBonusData, ragne) + GetBonus(speedBonusData, speed);
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
