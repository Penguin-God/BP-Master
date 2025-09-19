using System;
using System.Collections.Generic;
using System.Linq;

public enum TraitType
{
    None,
    AttackChanger,
    DefenseChanger,
    SpeedChanger,
}

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
    readonly SlotStorage<Champion> champions;
    readonly TraitTargetSelector targetFinder;
    readonly SlotStorage<bool> traitUseFlags;

    public event Action<StatChangeData> OnTraitApplied;

    public TraitController(SlotStorage<Champion> picks)
    {
        champions = picks;
        targetFinder = new TraitTargetSelector(picks.GetTeam(Team.Blue).Count());
        traitUseFlags = new SlotStorage<bool>(picks.GetTeam(Team.Blue).Count(), false);
    }

    public bool UseTrait(SlotData traitSlot, SlotData targetSlot)
    {
        if (IsTraitUsed(traitSlot)) return false;

        Champion executor = champions.GetSlot(traitSlot);
        var targetSlots = targetFinder.GetTargetSlots(traitSlot.Team, executor.TraitTargetRule, targetSlot);
        ExecuteTrait(executor.TraitExecutor, targetSlots);
        traitUseFlags.ChangeSlot(traitSlot, true);
        return true;
    }

    public bool IsTraitUsed(SlotData slot) => traitUseFlags.GetSlot(slot);

    void ExecuteTrait(TraitExecutor executor, IEnumerable<SlotData> slots)
    {
        foreach (var slot in slots)
        {
            var target = champions.GetSlot(slot);
            var before = target.StatData;
            executor.ExecteTrait(target);
            OnTraitApplied?.Invoke(new StatChangeData(slot, before, target.StatData));
        }
    }

    public int GetTeamSize(Team team) => champions.GetTeam(team).Count();
    public TraitTargetRule GetTargetRule(Team team, int index)
        => champions.GetSlot(new SlotData(team, index)).TraitTargetRule;
}
