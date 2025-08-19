using System.Collections.Generic;
using System.Linq;

public class TeamScoreCalculator
{
    readonly SortedDictionary<int, int> rangeBonusData;
    readonly SortedDictionary<int, int> speedBonusData;

    public TeamScoreCalculator(SortedDictionary<int, int> rangeBonusData, SortedDictionary<int, int> speedBonusData)
    {
        this.rangeBonusData = rangeBonusData;
        this.speedBonusData = speedBonusData;
    }

    public int CalculateScore(IEnumerable<Champion> team)
    {
        int defualtStat = team.Sum(x => x.Attack + x.Defense);
        int bonusStat = GetBonus(rangeBonusData, team.Sum(x => x.Range)) + GetBonus(speedBonusData, team.Sum(x => x.Speed));
        return defualtStat + bonusStat;
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
