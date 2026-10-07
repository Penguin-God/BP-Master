using System.Linq;
using System.Collections.Generic;

public class ChampionCatalog
{
    readonly IEnumerable<Champion> Champions;
    public ChampionCatalog(IEnumerable<Champion> champions) => Champions = champions;
    public Champion GetChampion(int id) => Champions.First(x => x.Id == id);
}
