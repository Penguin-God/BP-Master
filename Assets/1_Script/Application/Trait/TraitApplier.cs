using System.Collections.Generic;
using System.Linq;

public class TraitApplier
{
    readonly SlotStorage<ChampionStatus> statuses;
    readonly TraitTargetSelector targetSelector;
    public bool IsUse { get; set; }

    public TraitApplier(SlotStorage<ChampionStatus> statuses)
    {
        this.statuses = statuses;

        int teamSize = statuses.GetTeam(Team.Blue).Count();
        targetSelector = new TraitTargetSelector(teamSize);
    }

    public void Execute(TraitData traitData, SlotData targetSlot)
    {
        var executor = TraitExecutorFactory.CreateExecutor(traitData, statuses.GetSlot(targetSlot).Stat);
        IEnumerable<SlotData> targetSlots = targetSelector.GetTargetSlots(traitData.TargetRule, targetSlot);

        foreach (var slot in targetSlots)
        {
            var target = statuses.GetSlot(slot);
            executor.ExecuteTrait(target);
        }
        IsUse = true;
    }
}
