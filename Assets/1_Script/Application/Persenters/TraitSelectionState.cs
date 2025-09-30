public enum TraitSelectResult
{
    Faild,
    Select,
    Use
}

public class TraitSelectionState
{
    SlotData? selectSlot;
    public SlotData SelectSlot => selectSlot.Value;
    public bool IsSelect => selectSlot.HasValue;

    public readonly Team Team;
    public TraitSelectionState(Team team) => Team = team;

    public TraitSelectResult SelectTraitSlot(SlotData slot)
    {
        if (IsSelectable(slot.Team) == false) return TraitSelectResult.Faild;

        if (IsSelect)
        {
            selectSlot = null;
            return TraitSelectResult.Use;
        }
        else
        {
            selectSlot = slot;
            return TraitSelectResult.Select;
        }
    }

    bool IsSelectable(Team selectTeam) => (IsSelect == false && Team == selectTeam) || IsSelect;
}
