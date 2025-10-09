using System.Collections.Generic;
using System.Linq;

public class TraitUsePresenter
{
    readonly TraitUseFacade traitController;
    readonly SlotStorage<Champion> championStorage;
    TraitSlotFilter slotFilter;
    TraitSelectionState selectionState;
    
    public TraitUsePresenter(TraitUseFacade traitController, SlotStorage<Champion> champions, Team team)
    {
        this.traitController = traitController;
        championStorage = champions;
        slotFilter = new TraitSlotFilter(championStorage.GetTeam(Team.Blue).Count(), traitController);
        selectionState = new TraitSelectionState(team);
    }
    public void ChangeTeam(Team team) => selectionState = new TraitSelectionState(team);

    SlotData selectSlot;
    public void ClickChampion(SlotData slot)
    {
        var result = selectionState.SelectTraitSlot(slot);
        if (result == TraitSelectResult.Use) UseTrait(slot);
        else if (result == TraitSelectResult.Select) selectSlot = slot;
    }

    void UseTrait(SlotData targetSlot)
    {
        if (traitController.IsTraitUsed(selectSlot)) return;

        var traitData = championStorage.GetSlot(selectSlot).TraitDatas;
        traitController.UseTrait(selectSlot, targetSlot, traitData);
        selectSlot = default;
    }

    public IEnumerable<SlotData> GetClickableSlots()
    {
        if (selectionState.IsSelect == false) return slotFilter.FilteringUseableSlots(selectionState.Team);
        else
        {
            var targetSides = championStorage.GetSlot(selectSlot).TraitDatas.Select(x => x.TargetRule.TargetSide);
            return slotFilter.FilteringTargetSlots(selectionState.Team, targetSides);
        }
    }
}
