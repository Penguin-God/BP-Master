using System.Collections.Generic;
using System.Linq;

public class TeamMasteryApplier
{
    public void Apply(SlotStorage<ProGamer> gamers, SlotStorage<int> ids, SlotStorage<ChampionStatus> statuses)
    {
        foreach (var slot in gamers.GetAllSlotDatas())
        {
            int level = gamers.GetSlot(slot).GetMastery(ids.GetSlot(slot));
            var status = statuses.GetSlot(slot);
            status.ChangeStat(new MasteryCalculator().ApplyMastery(status.Stat, level));
        }
    }

    public void ApplyMastery(Dictionary<int, ChampionStatus> statuses, IEnumerable<ChampionMastery> masteries)
    {
        foreach (var mastery in masteries)
        {
            if (statuses.TryGetValue(mastery.ChampionId, out var status))
            {
                var oldStat = status.Stat;
                var newStat = new ChampionStatData(
                    oldStat.Attack + mastery.Level,
                    oldStat.Defense + mastery.Level,
                    oldStat.Speed
                );
                status.ChangeStat(newStat);
            }
        }
    }

    public void ApplyMastery(int[] ids, ChampionStatus[] statuses, IEnumerable<ChampionMastery> masteries)
    {
        for (int i = 0; i < ids.Length; i++)
        {
            if (masteries.Any(x => x.ChampionId == ids[i]))
            {
                var mastery = masteries.First(x => x.ChampionId == ids[i]);
                var status = statuses[i];
                var oldStat = status.Stat;
                var newStat = new ChampionStatData(
                    oldStat.Attack + mastery.Level,
                    oldStat.Defense + mastery.Level,
                    oldStat.Speed
                );
                status.ChangeStat(newStat);
            }
        }
    }
}
