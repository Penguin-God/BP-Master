using System.Collections.Generic;
using System.Linq;

public class ChampionManager
{
    readonly IEnumerable<Champion> allChampion;
    public IReadOnlyList<Champion> AllChampion => allChampion.ToArray();
    public IReadOnlyList<int> AllId => allChampion.Select(x => x.Id).ToList();

    public ChampionManager(IEnumerable<Champion> champions) => allChampion = champions;

    public Champion GetChampion(int id) => allChampion.First(x => x.Id == id);

    public IEnumerable<ChampionStatData> GetStats(IEnumerable<int> ids) => ids.Select(x => GetChampion(x).StatData);
}