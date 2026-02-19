using System;
using System.Collections.Generic;

public enum Team { Blue, Red, All }

public class BanPickStorage
{
    public SlotStorage<int> PickIds { get; private set; } = new();
    public readonly HashSet<int> SelectableIds = new();

    public BanPickStorage(IEnumerable<int> allIds) => SelectableIds = new HashSet<int>(allIds);

    void RemoveSelectableId(int id)
    {
        if (SelectableIds.Contains(id)) SelectableIds.Remove(id);
        else throw new ArgumentException($"선택 불가능한 ID : {id}");
    }

    public void Ban(Team team, int id) => RemoveSelectableId(id);

    public SlotData Pick(Team team, int id)
    {
        RemoveSelectableId(id);
        PickIds.AddSlot(team, id);
        return new SlotData(team, PickIds.GetTeamCount(team) - 1);
    }
}
