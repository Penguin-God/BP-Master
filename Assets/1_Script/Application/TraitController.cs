using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Graphs;
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
    public event Action<SlotData, StatDelta> OnTraitApplied;

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
        return true;
    }

    public bool IsTraitUsed(SlotData slot) => statuses.GetSlot(slot).IsUseTrait;

    void ExecuteTrait(TraitData traitData, IEnumerable<SlotData> slots)
    {
        var executor = TraitExecutorFactory.CreateExecutor(traitData);
        foreach (var slot in slots)
        {
            var delta = new TraitApplier().UseTrait(executor, statuses.GetSlot(slot));
            OnTraitApplied?.Invoke(slot, delta);
        }
    }
}

public readonly struct StatDelta
{
    public readonly int Attack;
    public readonly int Defense;
    public readonly int Speed;

    public StatDelta(int attack, int defense, int speed)
    {
        Attack = attack; 
        Defense = defense;
        Speed = speed;
    }
}

public class TraitApplier
{
    public StatDelta UseTrait(TraitExecutor executor, ChampionStatus target)
    {
        var before = target.StatData;
        executor.ExecuteTrait(target);

        var after = target.StatData;
        return new StatDelta(
            attack: after.Attack - before.Attack,
            defense: after.Defense - before.Defense,
            speed: after.Speed - before.Speed
        );
    }
}