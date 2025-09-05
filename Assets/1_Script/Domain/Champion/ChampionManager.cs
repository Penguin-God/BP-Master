using System.Collections.Generic;
using System.Linq;

public class ChampionManagerA
{
    readonly IEnumerable<Champion> allChampion;
    public IReadOnlyList<Champion> AllChampion => allChampion.ToArray();
    public IReadOnlyList<int> AllId => allChampion.Select(x => x.Id).ToList();

    public ChampionManagerA(Champion[] champions) => allChampion = champions;

    // 모든 챔 데이터 다 들고있는데 못찾는건 말도 안되는 상황이라 First() 사용
    public Champion GetChampion(int id) => allChampion.First(x => x.Id == id);

    public IEnumerable<ChampionStatData> GetStats(IEnumerable<int> ids) => ids.Select(x => GetChampion(x).StatData);
}