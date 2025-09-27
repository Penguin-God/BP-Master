
public class TeamMasteryApplier
{
    public void Apply(SlotStorage<ProGamer> gamers, SlotStorage<int> ids, SlotStorage<ChampionStatus> statuses)
    {
        foreach (var slot in gamers.GetAllSlotDatas())
        {
            int level = gamers.GetSlot(slot).GetMastery(ids.GetSlot(slot));
            var status = statuses.GetSlot(slot);
            status.ChangeStat(new MasteryApplier().ApplyMastery(status.Stat, level));
        }
    }
}
