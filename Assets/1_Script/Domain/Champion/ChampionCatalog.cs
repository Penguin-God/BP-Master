using System.Collections.Generic;
using System.Linq;

public class ChampionCatalog
{
    readonly IEnumerable<Champion> allChampion;
    public IReadOnlyList<int> AllId => allChampion.Select(x => x.Id).ToList();

    public ChampionCatalog(IEnumerable<Champion> champions) => allChampion = champions;

    public Champion GetChampion(int id) => Clone(allChampion.First(x => x.Id == id));
    Champion Clone(Champion src) => new Champion(src.Id, src.Name, src.StatData, src.TraitData);
}