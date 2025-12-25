using System;
using System.Collections.Generic;

public readonly struct SelectInfo
{
    public readonly Team Team;
    public readonly SelectType Select;
    public readonly int Id;

    public SelectInfo(Team team, SelectType select, int id)
    {
        Team = team;
        Select = select;
        Id = id;
    }
}

public enum Team { Blue, Red, All }
public enum SelectType { Ban, Pick}

public class GameBanPickStorage
{
    readonly Dictionary<Team, HashSet<int>> banStorage = new();
    public SlotStorage<int> PickIds { get; set; } = new();

    public event Action<Team, int> OnBan;
    public event Action<SlotData, int> OnPick;

    public readonly HashSet<int> SelectableIds = new();

    public GameBanPickStorage(IEnumerable<int> allIds)
    {
        SelectableIds = new HashSet<int>(allIds);
        banStorage.Add(Team.Red, new());
        banStorage.Add(Team.Blue, new());
    }

    public bool CanSelected(int id) => SelectableIds.Contains(id);

    public void SaveSelect(SelectInfo info)
    {
        if (CanSelected(info.Id) == false) return;
        SelectableIds.Remove(info.Id);

        if (info.Select == SelectType.Ban) Ban(info);
        else Pick(info);
    }

    void Ban(SelectInfo info)
    {
        banStorage[info.Team].Add(info.Id);
        OnBan?.Invoke(info.Team, info.Id);
    }

    void Pick(SelectInfo info)
    {
        PickIds.AddSlot(info.Team, info.Id);
        OnPick?.Invoke(new SlotData(info.Team, PickIds.GetTeamCount(info.Team) - 1), info.Id);
    }
}
