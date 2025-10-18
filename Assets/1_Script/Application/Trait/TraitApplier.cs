using System.Collections.Generic;
using System.Linq;

public class TraitApplier
{
    readonly SlotStorage<ChampionStatus> statuses;
    readonly TraitTargetFinder targetSelector;
    public bool IsUse { get; set; }

    public TraitApplier(SlotStorage<ChampionStatus> statuses)
    {
        this.statuses = statuses;

        int teamSize = statuses.GetTeam(Team.Blue).Count();
        targetSelector = new TraitTargetFinder(teamSize);
    }

    public void Execute(TraitData traitData, SlotData targetSlot, SlotData useSlot)
    {
        var executor = new TraitExecutorFactory().CreateExecutor(traitData, statuses.GetSlot(useSlot).Stat);
        IEnumerable<SlotData> targetSlots = targetSelector.GetTargetSlots(traitData.TargetRule, targetSlot);

        foreach (var slot in targetSlots)
        {
            var target = statuses.GetSlot(slot);
            executor.ExecuteTrait(target);
        }
        IsUse = true;
    }
}
