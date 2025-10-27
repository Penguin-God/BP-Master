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
    readonly Dictionary<Team, BanStorage> storage = new();
    public SlotStorage<int> PickIds { get; set; } = new();

    public event Action<Team, int> OnBan;
    public event Action<SlotData, int> OnPick;

    public readonly HashSet<int> SelectableIds = new();

    public event Action<int> OnSwap;

    public GameBanPickStorage(IEnumerable<int> allIds)
    {
        SelectableIds = new HashSet<int>(allIds);
        storage.Add(Team.Red, new());
        storage.Add(Team.Blue, new());
    }

    public bool CanSelected(int id) => SelectableIds.Contains(id);

    public void SaveSelect(SelectInfo info)
    {
        if (CanSelected(info.Id) == false) return;
        SelectableIds.Remove(info.Id);

        if (info.Select == SelectType.Ban)
        {
            storage[info.Team].SaveBan(info.Id);
            OnBan?.Invoke(info.Team, info.Id);
        }
        else
        {
            PickIds.AddSlot(info.Team, info.Id);
            OnPick?.Invoke(new SlotData(info.Team, PickIds.Count(info.Team) - 1), info.Id);
        }
    }

    public IReadOnlyList<int> GetStorage(Team team, SelectType select)
    {
        if (select == SelectType.Ban)
        {
            return storage[team].Bans;
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
    }
}

public class BanStorage
{
    List<int> bans = new List<int>();
    public IReadOnlyList<int> Bans => bans;

    public void SaveBan(int id) => bans.Add(id);
}