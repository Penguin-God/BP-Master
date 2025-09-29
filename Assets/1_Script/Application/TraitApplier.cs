using System.Collections.Generic;
using System.Linq;

public class TraitApplier
{
    readonly SlotStorage<ChampionStatus> statuses;
    readonly TraitTargetSelector targetSelector;

    public TraitApplier(SlotStorage<ChampionStatus> statuses)
    {
        this.statuses = statuses;

        int teamSize = statuses.GetTeam(Team.Blue).Count();
        targetSelector = new TraitTargetSelector(teamSize);
    }

    public void Execute(TraitData traitData, SlotData targetSlot, TargetRange range)
    {
        var executor = TraitExecutorFactory.CreateExecutor(traitData);
        IEnumerable<SlotData> targetSlots = targetSelector.GetTargetSlots(range, targetSlot);

        foreach (var slot in targetSlots)
        {
            var target = statuses.GetSlot(slot);
            executor.ExecuteTrait(target);
        }
    }
}