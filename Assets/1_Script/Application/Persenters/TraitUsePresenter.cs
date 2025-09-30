using System.Collections.Generic;
using System.Linq;

public enum TraitClickResult
{
    Faild,
    Select,
    Use
}

public class TraitUsePresenter // 타겟들 다 포함
{
    readonly TraitUseFacade traitController;
    readonly SlotStorage<Champion> championStorage;
    readonly TraitSlotFilter slotFilter;
    Team currentTeam = Team.All;
    
    public TraitUsePresenter(TraitUseFacade traitController, SlotStorage<Champion> champions)
    {
        this.traitController = traitController;
        championStorage = champions;
        slotFilter = new TraitSlotFilter(championStorage.GetTeam(Team.Blue).Count(), traitController);
    }
    public void ChangeTeam(Team team) => currentTeam = team;

    SlotData? selected; // 선택된 시전자
    public bool IsSelect => selected.HasValue;

    public void ClickChampion(SlotData slot)
    {
        if (IsClickable(slot.Team) == false) return;

        if (IsSelect) UseTrait(slot);
        else selected = slot;
    }

    bool IsClickable(Team buttonTeam) // 나중에는 타겟 범위까지 판단해야 됨
    {
        return (IsSelect == false && currentTeam == buttonTeam) || IsSelect;
    }

    void UseTrait(SlotData targetSlot)
    {
        if (traitController.IsTraitUsed(selected.Value)) return;

        var traitData = championStorage.GetSlot(selected.Value).TraitData;
        traitController.UseTrait(selected.Value, targetSlot, traitData);
        selected = null;
    }

    public IEnumerable<SlotData> GetClickableSlots()
    {
        if (IsSelect == false) return slotFilter.FilteringUseableSlots(currentTeam);
        else
        {
            var targetSide = championStorage.GetSlot(selected.Value).TraitData.TargetRule.TargetSide;
            return slotFilter.FilteringTargetSlots(currentTeam, targetSide);
        }
    }
}
