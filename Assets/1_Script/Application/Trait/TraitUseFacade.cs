using System;
using System.Collections.Generic;
using System.Linq;

public class TraitUseFacade
{
    readonly SlotStorage<ChampionStatus> statusSlots;
    public event Action<SlotData> OnUseTrait;

    public TraitUseFacade(SlotStorage<ChampionStatus> statusSlots) => this.statusSlots = statusSlots;

    public void UseTrait(SlotData traitSlot, IEnumerable<SlotData> targetSlots, IEnumerable<TraitData> traitDatas)
    {
        var targets = targetSlots.Select(x => statusSlots.GetSlot(x));
        foreach (var trait in traitDatas)
        {
            var executor = new TraitExecutorFactory().CreateExecutor(trait, statusSlots.GetSlot(traitSlot).Stat);
            executor.ExecuteTrait(targets);
        }
        OnUseTrait?.Invoke(traitSlot);
    }
}