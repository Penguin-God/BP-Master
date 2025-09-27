using System;
using System.Collections.Generic;
using System.Linq;
using static PlasticGui.WorkspaceWindow.Merge.MergeInProgress;

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
    readonly Dictionary<Team, TeamBanPickStorage> storage = new();
    public SlotStorage<int> PickIds { get; set; } = new();

    public event Action<Team, int> OnBan;
    public event Action<Team, int> OnPick;

    readonly HashSet<int> selectableIds = new();

    public event Action<int> OnSwap;

    public GameBanPickStorage(IEnumerable<int> allIds)
    {
        selectableIds = new HashSet<int>(allIds);
        storage.Add(Team.Red, new());
        storage.Add(Team.Blue, new());
    }

    public bool CanSelected(int id) => selectableIds.Contains(id);

    public void SaveSelect(SelectInfo info)
    {
        if (CanSelected(info.Id) == false) return;
        selectableIds.Remove(info.Id);

        //storage[info.Team].SaveSelect(info.Select, info.Id);
        //if (info.Select == SelectType.Ban) OnBan?.Invoke(info.Team, info.Id);
        //else OnPick?.Invoke(info.Team, info.Id);

        if (info.Select == SelectType.Ban)
        {
            storage[info.Team].SaveSelect(info.Select, info.Id);
            OnBan?.Invoke(info.Team, info.Id);
        }
        else
        {
            PickIds.AddSlot(info.Team, info.Id);
            OnPick?.Invoke(info.Team, info.Id);
        }
    }
    // public IReadOnlyList<int> GetStorage(Team team, SelectType select) => storage[team].GetStorage(select);
    public IReadOnlyList<int> GetStorage(Team team, SelectType select)
    {
        if (select == SelectType.Ban)
        {
            return storage[team].GetStorage(select);
        }
        else
        {
            return PickIds.GetTeam(team).ToList();
        }
    }
    public void Swap(Team team, int index1, int index2)
    {
        var slot1 = new SlotData(team, index1);
        var slot2 = new SlotData(team, index2);

        int id1 = PickIds.GetSlot(slot1);
        int id2 = PickIds.GetSlot(slot2);

        PickIds.ChangeSlot(slot1, id2);
        PickIds.ChangeSlot(slot2, id1);
        // storage[team].Swap(index1, index2);
    }
}

public class TeamBanPickStorage
{
    readonly Dictionary<SelectType, List<int>> storage;

    public TeamBanPickStorage()
    {
        storage = new Dictionary<SelectType, List<int>>() 
        {
            { SelectType.Ban, new List<int>()},
            { SelectType.Pick, new List<int>()}
        };
    }

    public void SaveSelect(SelectType select, int id) => storage[select].Add(id);
    public IReadOnlyList<int> GetStorage(SelectType select) => storage[select];

    public void Swap(int index1, int index2)
    {
        if (index1 == index2) return;

        var list = storage[SelectType.Pick];
        (list[index1], list[index2]) = (list[index2], list[index1]);
    }
}