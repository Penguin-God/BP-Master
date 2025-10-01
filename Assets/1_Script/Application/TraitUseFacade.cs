using System;
using System.Collections.Generic;

public class TraitUseFacade
{
    readonly SlotStorage<ChampionStatus> statuses;
    readonly TraitApplier applier;

    public event Action<Team> OnTraitUsed;

    public TraitUseFacade(SlotStorage<ChampionStatus> statuses)
    {
        this.statuses = statuses;
        applier = new TraitApplier(statuses);
    }

    public void UseTrait(SlotData traitSlot, SlotData targetSlot, TraitData traitData)
    {
        if (IsTraitUsed(traitSlot)) return;

        statuses.GetSlot(traitSlot).UseTrait(); 
        applier.Execute(traitData, targetSlot);
        OnTraitUsed?.Invoke(traitSlot.Team);
    }

    public void UseTrait(SlotData traitSlot, SlotData targetSlot, IEnumerable<TraitData> traitDatas)
    {
        if (IsTraitUsed(traitSlot)) return;

        statuses.GetSlot(traitSlot).UseTrait();
        foreach (var data in traitDatas)
            applier.Execute(data, targetSlot);
        OnTraitUsed?.Invoke(traitSlot.Team);
    }

    public bool IsTraitUsed(SlotData slot) => statuses.GetSlot(slot).IsUseTrait;
}