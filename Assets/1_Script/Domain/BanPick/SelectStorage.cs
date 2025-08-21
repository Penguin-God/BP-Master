using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameSelectStorage
{
    HashSet<int> selectChampions = new();
    readonly public TeamSelectStorage BansStorage = new();
    readonly public TeamSelectStorage PickStorage = new();

    public IReadOnlyList<int> SelectChampions => selectChampions.ToArray();

    public void SaveSelectChampion(GamePhase phase, Team team, int id)
    {
        GetStorage(phase).Add(team, id);
        selectChampions.Add(id);
    }

    public TeamSelectStorage GetStorage(GamePhase phase) => phase == GamePhase.Ban ? BansStorage : PickStorage;
}

public enum Team { Blue, Red, All }
public enum SelectType { Ban, Pick}
public class TeamSelectStorage
{
    Dictionary<Team, List<int>> storage = new();

    public TeamSelectStorage()
    {
        storage.Add(Team.Blue, new List<int>());
        storage.Add(Team.Red, new List<int>());
    }

    public void Add(Team team, int id) => storage[team].Add(id);
    public int GetCount(Team team) => storage[team].Count;
}

public class GameBanPickStorage
{
    readonly Dictionary<Team, TeamBanPickStorage> storage = new();
    readonly HashSet<int> allSelecteds = new();
    readonly HashSet<int> selectableIds = new();

    public IReadOnlyList<int> SelectableIds => selectableIds.ToList();

    public GameBanPickStorage(IEnumerable<int> allIds)
    {
        selectableIds = new HashSet<int>(allIds);
        storage.Add(Team.Red, new());
        storage.Add(Team.Blue, new());
    }

    public bool SaveSelect(SelectInfo info)
    {
        if(selectableIds.Contains(info.Id) == false) return false;

        selectableIds.Remove(info.Id);
        allSelecteds.Add(info.Id);
        storage[info.Team].SaveSelect(info.Select, info.Id);
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