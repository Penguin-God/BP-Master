
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
    public TraitSelectionState selectionState;
    public Team Team => selectionState.Team;

    public TraitUsePresenter(Team team) => selectionState = new TraitSelectionState(team);

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
}
