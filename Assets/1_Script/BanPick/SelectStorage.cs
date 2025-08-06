
using System.Collections.Generic;
using System.Linq;

public class ChampionSelectStorage
{
    HashSet<int> selectChampions = new();
    readonly public TeamSelectStorage BansStorage = new();
    readonly public TeamSelectStorage PickStorage = new();

    public IReadOnlyList<int> SelectChampions => selectChampions.ToArray();

    public void SaveSelectChampion(BanPcikPhase phase, Team team, int id)
    {
        GetStorage(phase).Add(team, id);
        selectChampions.Add(id);
    }

    public TeamSelectStorage GetStorage(BanPcikPhase phase) => phase == BanPcikPhase.Ban ? BansStorage : PickStorage;
}

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