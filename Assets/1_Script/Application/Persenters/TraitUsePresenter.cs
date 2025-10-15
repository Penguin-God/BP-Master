using System.Collections.Generic;
using System.Linq;

public readonly struct TraitUseSlotData
{
    public readonly SlotData UseSlot;
    public readonly SlotData TargetSlot;

    public TraitUseSlotData(SlotData useSlot, SlotData targetSlot)
    {
        this.UseSlot = useSlot;
        this.TargetSlot = targetSlot;
    }
}

public class TraitUsePresenter
{
    readonly TraitUseFacade traitController;
    readonly SlotStorage<IEnumerable<TraitData>> traitDatas;
    TraitSlotFilter slotFilter;
    public TraitSelectionState selectionState;
    public Team Team => selectionState.Team;

    public TraitUsePresenter(TraitUseFacade traitController, SlotStorage<IEnumerable<TraitData>> traits, Team team)
    {
        this.traitController = traitController;
        traitDatas = traits;
        slotFilter = new TraitSlotFilter(traitDatas.GetTeam(Team.Blue).Count(), traitController);
        selectionState = new TraitSelectionState(team);
    }

    public TraitUsePresenter(SlotStorage<IEnumerable<TraitData>> traits, Team team)
    {
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

    public bool ClickChampion(SlotData slot, out TraitUseSlotData traitUseSlotData)
    {
        traitUseSlotData = default;
        SlotData? result = selectionState.ClickTraitSlot(slot);
        if (result.HasValue)
        {
            traitUseSlotData = new TraitUseSlotData(result.Value, slot);
            return true;
        }
        else return false;
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
