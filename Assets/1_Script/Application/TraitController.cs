using System;
using System.Collections.Generic;
using System.Linq;
public readonly struct StatChangeData
{
    public readonly SlotData Slot;
    public readonly ChampionStatData Before;
    public readonly ChampionStatData After;

    public StatChangeData(SlotData slot, ChampionStatData before, ChampionStatData after)
    {
        Slot = slot;
        Before = before;
        After = after;
    }
}


public class TraitController
{
    readonly SlotStorage<ChampionStatus> statuses;
    readonly TraitTargetSelector targetFinder;

    public event Action<Team> OnTraitUsed;
    public event Action<StatChangeData> OnTraitApplied;

    public TraitController(SlotStorage<ChampionStatus> statuses)
    {
        this.statuses = statuses;

        int teamSize = statuses.GetTeam(Team.Blue).Count();
        targetFinder = new TraitTargetSelector(teamSize);
    }

    public bool UseTrait(SlotData traitSlot, SlotData targetSlot, TraitData traitData, TargetRange range)
    {
        if (IsTraitUsed(traitSlot)) return false;

        statuses.GetSlot(traitSlot).UseTrait();
        ExecuteTrait(traitData, targetFinder.GetTargetSlots(range, targetSlot));
        OnTraitUsed?.Invoke(traitSlot.Team);
        return true;
    }

    public bool IsTraitUsed(SlotData slot) => statuses.GetSlot(slot).IsUseTrait;

    void ExecuteTrait(TraitData traitData, IEnumerable<SlotData> slots)
    {
        var executor = TraitExecutorFactory.CreateExecutor(traitData);
        foreach (var slot in slots)
        {
            var target = statuses.GetSlot(slot);
            var before = target.StatData;
            executor.ExecuteTrait(target);
            OnTraitApplied?.Invoke(new StatChangeData(slot, before, target.StatData));
        }
    }
}