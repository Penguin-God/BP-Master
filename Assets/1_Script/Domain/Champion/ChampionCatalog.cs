using System.Collections.Generic;
using System.Linq;

public class ChampionCatalog
{
    readonly IEnumerable<Champion> allChampion;
    public IReadOnlyList<int> AllId => allChampion.Select(x => x.Id).ToList();

    public ChampionCatalog(IEnumerable<Champion> champions) => allChampion = champions;
}