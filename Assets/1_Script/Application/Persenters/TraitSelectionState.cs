public enum TraitSelectResult
{
    Faild,
    Select,
    Use
}

public class TraitSelectionState
{
    public bool IsSelect { get; private set; }

    public readonly Team Team;
    public TraitSelectionState(Team team) => Team = team;

    public TraitSelectResult SelectTraitSlot(SlotData slot)
    {
        if (IsSelectable(slot.Team) == false) return TraitSelectResult.Faild;

        var result = IsSelect ? TraitSelectResult.Use : TraitSelectResult.Select;
        IsSelect = !IsSelect;
        return result;
    }

    bool IsSelectable(Team selectTeam) => (IsSelect == false && Team == selectTeam) || IsSelect;
}
