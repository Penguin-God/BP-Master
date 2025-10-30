
using System.Collections.Generic;

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
}
