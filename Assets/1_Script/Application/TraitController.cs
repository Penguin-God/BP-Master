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
    readonly SlotStorage<Champion> champions;
    readonly SlotStorage<ChampionStatus> statuses;
    readonly TraitTargetSelector targetFinder;
    
    public event Action<StatChangeData> OnTraitApplied;

    public TraitController(SlotStorage<Champion> picks, SlotStorage<ChampionStatus> statuses)
    {
        this.champions = picks;
        this.statuses = statuses;

        int teamSize = picks.GetTeam(Team.Blue).Count();
        targetFinder = new TraitTargetSelector(teamSize);
    }

    public bool UseTrait(SlotData traitSlot, SlotData targetSlot)
    {
        if (IsTraitUsed(traitSlot)) return false;

        Champion user = champions.GetSlot(traitSlot); // 룰/데이터는 Champion에서
        var targetSlots = targetFinder.GetTargetSlots(user.TraitTargetRule.TargetRange, targetSlot);

        ExecuteTrait(user.TraitData, targetSlots);

        statuses.GetSlot(traitSlot).UseTrait();
        return true;
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
            var targetStatus = statuses.GetSlot(slot);
            var before = targetStatus.StatData;

            executor.ExecuteTrait(targetStatus);

            OnTraitApplied?.Invoke(new StatChangeData(slot, before, targetStatus.StatData));
        }
    }

    // 조회(그대로 유지, 룰은 Champion에서 읽음)
    public int GetTeamSize(Team team) => champions.GetTeam(team).Count();
    public TraitTargetRule GetTargetRule(Team team, int index)
        => champions.GetSlot(new SlotData(team, index)).TraitTargetRule;
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