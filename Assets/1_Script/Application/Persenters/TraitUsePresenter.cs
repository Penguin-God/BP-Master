using System.Collections.Generic;
using System.Linq;

public class TraitUsePresenter
{
    readonly TraitUseFacade traitController;
    readonly SlotStorage<IEnumerable<TraitData>> traitDatas;
    TraitSlotFilter slotFilter;
    TraitSelectionState selectionState;
    
    public TraitUsePresenter(TraitUseFacade traitController, SlotStorage<IEnumerable<TraitData>> traits, Team team)
    {
        this.traitController = traitController;
        traitDatas = traits;
        slotFilter = new TraitSlotFilter(traitDatas.GetTeam(Team.Blue).Count(), traitController);
        selectionState = new TraitSelectionState(team);
    }
    public void ChangeTeam(Team team) => selectionState = new TraitSelectionState(team);

    public void ClickChampion(SlotData slot)
    {
        SlotData? result = selectionState.ClickTraitSlot(slot);
        if (result.HasValue) UseTrait(result.Value, slot);
    }

    void UseTrait(SlotData useSlot, SlotData targetSlot)
    {
        var traitData = traitDatas.GetSlot(useSlot);
        traitController.UseTrait(useSlot, targetSlot, traitData);
    }

    public IEnumerable<SlotData> GetClickableSlots()
    {
        return GetSlots();
    }

    IEnumerable<SlotData> GetSlots()
    {
        if (selectionState.UseTurn == false) return slotFilter.FilteringUseableSlots(selectionState.Team);
        else
        {
            var targetSides = traitDatas.GetSlot(selectionState.UseSlot).Select(x => x.TargetRule.TargetSide);
            return slotFilter.FilteringTargetSlots(selectionState.Team, targetSides);
        }
    }
}
