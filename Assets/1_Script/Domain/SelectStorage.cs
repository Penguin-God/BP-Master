using System;
using System.Collections.Generic;
using System.Linq;

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

    public event Action<Team, int> OnBan;
    public event Action<Team, int> OnPick;

    readonly HashSet<int> allSelecteds = new();
    readonly HashSet<int> selectableIds = new();
    public IReadOnlyDictionary<SlotData, int> PickBySlot =>
    storage
        .SelectMany(kv => kv.Value.GetStorage(SelectType.Pick)
                                  .Select((id, index) => new KeyValuePair<SlotData, int>(new SlotData(kv.Key, index), id)))
        .ToDictionary(p => p.Key, p => p.Value);

    public IReadOnlyList<int> SelectableIds => selectableIds.ToList();

    public GameBanPickStorage(IEnumerable<int> allIds)
    {
        selectableIds = new HashSet<int>(allIds);
        storage.Add(Team.Red, new());
        storage.Add(Team.Blue, new());
    }

    public bool CanSelected(int id) => selectableIds.Contains(id);

    public bool SaveSelect(SelectInfo info)
    {
        if(CanSelected(info.Id) == false) return false;

        selectableIds.Remove(info.Id);
        allSelecteds.Add(info.Id);
        storage[info.Team].SaveSelect(info.Select, info.Id);
        if (info.Select == SelectType.Ban) OnBan?.Invoke(info.Team, info.Id);
        else OnPick?.Invoke(info.Team, info.Id);
        return true;
    }
    public IReadOnlyList<int> GetStorage(Team team, SelectType select) => storage[team].GetStorage(select);
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