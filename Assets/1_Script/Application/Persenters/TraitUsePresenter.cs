using System.Collections.Generic;
using System.Linq;

public class TraitUsePresenter
{
    readonly TraitUseFacade traitController;
    readonly SlotStorage<Champion> championStorage;
    TraitSlotFilter slotFilter;
    TraitSelectionState selectionState;
    Team currentTeam = Team.All;
    
    public TraitUsePresenter(TraitUseFacade traitController, SlotStorage<Champion> champions)
    {
        this.traitController = traitController;
        championStorage = champions;
        slotFilter = new TraitSlotFilter(championStorage.GetTeam(Team.Blue).Count(), traitController);
    }
    public void ChangeTeam(Team team)
    {
        currentTeam = team;
        selectionState = new TraitSelectionState(team);
    }

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

        var traitData = championStorage.GetSlot(selectSlot).TraitData;
        traitController.UseTrait(selectSlot, targetSlot, traitData);
        selectSlot = default;
    }

    public IEnumerable<SlotData> GetClickableSlots()
    {
        if (selectionState.IsSelect == false) return slotFilter.FilteringUseableSlots(currentTeam);
        else
        {
            var targetSide = championStorage.GetSlot(selectSlot).TraitData.TargetRule.TargetSide;
            return slotFilter.FilteringTargetSlots(currentTeam, targetSide);
        }
    }
}
