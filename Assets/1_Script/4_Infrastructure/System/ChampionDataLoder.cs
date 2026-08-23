using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ChampionDataLoder
{
    static ChampionSO[] _allChampion;
    public static IReadOnlyList<ChampionSO> AllChampions
    {
        get
        {
            _allChampion ??= Resources.LoadAll<ChampionSO>("SO/Data/Champions");
            return _allChampion;
        }
    }
    public static IReadOnlyDictionary<int, string> NameCatalog => _allChampion.ToDictionary(x => x.Id, x => x.ChampionName);

    public static IEnumerable<int> AllId => AllChampions.Select(x => x.Id);
    public static ChampionCatalog GetCatalog() => new ChampionCatalog(AllChampions.Select(x => x.CreateChampion()));
    public static ChampionSO GetChampionData(int id) => AllChampions.First(x => x.Id == id);   
}