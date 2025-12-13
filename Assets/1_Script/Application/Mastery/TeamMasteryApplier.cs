using System.Collections.Generic;
using System.Linq;

public class TeamMasteryApplier
{
    public void ApplyMastery(int[] ids, ChampionStatus[] statuses, IEnumerable<ChampionMastery> masteries)
    {
        for (int i = 0; i < ids.Length; i++)
        {
            if (masteries.Any(x => x.ChampionId == ids[i]))
            {
                var mastery = masteries.First(x => x.ChampionId == ids[i]);
                ApplyStatChange(statuses[i], mastery.Level);
            }
        }
    }

    public void ApplyStatChange(ChampionStatus status, int masteryLevel)
    {
        var oldStat = status.Stat;
        var newStat = new ChampionStatData(
            oldStat.Attack + masteryLevel,
            oldStat.Defense + masteryLevel,
            oldStat.Speed
        );
        status.ChangeStat(newStat);
    }
}
