using System.Collections.Generic;
using System.Linq;

public class TeamBonusCalculator
{
    readonly SortedDictionary<int, int> attackBonusData;
    readonly SortedDictionary<int, int> defenseBonusData;
    readonly SortedDictionary<int, int> rangeBonusData;
    readonly SortedDictionary<int, int> speedBonusData;

    public TeamBonusCalculator(SortedDictionary<int, int> attackBonusData, SortedDictionary<int, int> defenseBonusData, SortedDictionary<int, int> rangeBonusData, SortedDictionary<int, int> speedBonusData)
    {
        this.attackBonusData = attackBonusData;
        this.defenseBonusData = defenseBonusData;
        this.rangeBonusData = rangeBonusData;
        this.speedBonusData = speedBonusData;
    }

    public int CalculateTeamBonus(IEnumerable<Champion> team)
    {
        return GetBonus(attackBonusData, team.Sum(x => x.Attack)) + GetBonus(defenseBonusData, team.Sum(x => x.Defense)) + GetBonus(rangeBonusData, team.Sum(x => x.Range)) + GetBonus(speedBonusData, team.Sum(x => x.Speed));
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
