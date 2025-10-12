
public class TraitSelectionState
{
    public readonly Team Team;
    SlotData? useSlot;
    public SlotData UseSlot => useSlot.Value;
    public bool IsSelect => useSlot.HasValue;

    public TraitSelectionState(Team team) => Team = team;

    public SlotData? ClickTraitSlot(SlotData slot)
    {
        if (IsSelectable(slot.Team) == false) return null;

        if (IsSelect)
        {
            var result = UseSlot;
            useSlot = null;
            return result;
        }
        else
        {
            useSlot = slot;
            return null;
        }
    }

    bool IsSelectable(Team selectTeam) => (IsSelect == false && Team == selectTeam) || IsSelect;
}
