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

    public void ClickChampion(SlotData slot)
    {
        var result = selectionState.SelectTraitSlot(slot);
        if (result == TraitSelectResult.Use) UseTrait(slot);
    }

    void UseTrait(SlotData targetSlot)
    {
        if (traitController.IsTraitUsed(selectionState.SelectSlot)) return;

        var traitData = championStorage.GetSlot(selectionState.SelectSlot).TraitData;
        traitController.UseTrait(selectionState.SelectSlot, targetSlot, traitData);
    }

    public IEnumerable<SlotData> GetClickableSlots()
    {
        if (selectionState.IsSelect == false) return slotFilter.FilteringUseableSlots(currentTeam);
        else
        {
            var targetSide = championStorage.GetSlot(selectionState.SelectSlot).TraitData.TargetRule.TargetSide;
            return slotFilter.FilteringTargetSlots(currentTeam, targetSide);
        }
    }
}
