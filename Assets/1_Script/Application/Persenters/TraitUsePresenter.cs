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
    bool IsSelect => selected.HasValue;

    public TraitClickResult ClickChampion(SlotData slot)
    {
        if (IsClickable(slot.Team) == false) return TraitClickResult.Faild;

        if (IsSelect) return UseTrait(slot);
        else
        {
            selected = slot;
            return TraitClickResult.Select;
        }
    }

    bool IsClickable(Team buttonTeam) // 나중에는 타겟 범위까지 판단해야 됨
    {
        return (IsSelect == false && currentTeam == buttonTeam) || IsSelect;
    }

    TraitClickResult UseTrait(SlotData targetSlot)
    {
        var sel = selected.Value;
        var traitData = championStorage.GetSlot(sel).TraitData;
        if (traitController.UseTrait(selected.Value, targetSlot, traitData))
        {
            selected = null;
            return TraitClickResult.Use;
        }
        else return TraitClickResult.Faild;
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
