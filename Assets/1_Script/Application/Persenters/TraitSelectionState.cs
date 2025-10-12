public enum TraitSelectResult
{
    Faild,
    Select,
    Use
}

public class TraitSelectionState
{
    public readonly Team Team;
    SlotData? useSlot;
    public SlotData UseSlot => useSlot.Value;
    public bool IsSelect => useSlot.HasValue;
    public TraitSelectionState(Team team) => Team = team;

    public TraitSelectResult SelectTraitSlot(SlotData slot)
    {
        if (IsSelectable(slot.Team) == false) return TraitSelectResult.Faild;

        if (IsSelect)
        {
            useSlot = null;
            return TraitSelectResult.Use;
        }
        else
        {
            useSlot = slot;
            return TraitSelectResult.Select;
        }
    }

    bool IsSelectable(Team selectTeam) => (IsSelect == false && Team == selectTeam) || IsSelect;
}
