using System.Collections.Generic;
using System.Linq;

public class ChampionCatalog
{
    readonly IEnumerable<Champion> allChampion;
    public IReadOnlyList<int> AllId => allChampion.Select(x => x.Id).ToList();

    public ChampionCatalog(IEnumerable<Champion> champions) => allChampion = champions;

    public Champion GetChampion(int id) => Clone(allChampion.First(x => x.Id == id));
    // 현재는 스탯만 바뀌니까 이 정도만 클론하면 충분
    Champion Clone(Champion src) => new Champion(src.Id, src.Name, src.StatData, src.TraitTargetRule, src.TraitData);
}