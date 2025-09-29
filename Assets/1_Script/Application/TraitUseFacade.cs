using System;

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

    public bool UseTrait(SlotData traitSlot, SlotData targetSlot, TraitData traitData, TargetRange range)
    {
        if (IsTraitUsed(traitSlot)) return false;

        statuses.GetSlot(traitSlot).UseTrait();
        applier.Execute(traitData, targetSlot, range);
        OnTraitUsed?.Invoke(traitSlot.Team);
        return true;
    }

    public bool IsTraitUsed(SlotData slot) => statuses.GetSlot(slot).IsUseTrait;
}