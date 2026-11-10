using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class BonusTextBulider
{
    public string BuildLineText(SortedDictionary<int, int> thresholdToBonus) => string.Join(", ", thresholdToBonus.Select(pair => $"{pair.Key}이상 +{pair.Value}"));

    public string BuildBonusAllText(SortedDictionary<int, int> attackBonus, SortedDictionary<int, int> defenseBonus)
    {
        var line = Environment.NewLine;
        var sb = new StringBuilder();
        sb.Append("보너스 점수(누적 X)").Append(line);
        sb.Append("공격력 : ").Append(BuildLineText(attackBonus)).Append(line);
        sb.Append("방어력 : ").Append(BuildLineText(defenseBonus));

        return sb.ToString();
    }
}
