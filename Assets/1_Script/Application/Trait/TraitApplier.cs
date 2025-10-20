using System.Collections.Generic;

public class TraitApplier
{
    readonly SlotStorage<ChampionStatus> statuses;
    public bool IsUse { get; set; }

    readonly SlotData Slot;
    public TraitApplier(SlotStorage<ChampionStatus> statuses, SlotData slotData)
    {
        this.statuses = statuses;
        Slot = slotData;
    }

    public void Execute(TraitData traitData, IEnumerable<SlotData> targetSlots)
    {
        var executor = new TraitExecutorFactory().CreateExecutor(traitData, statuses.GetSlot(Slot).Stat);
        foreach (var slot in targetSlots)
        {
            var target = statuses.GetSlot(slot);
            executor.ExecuteTrait(target);
        }
        IsUse = true;
    }
}
