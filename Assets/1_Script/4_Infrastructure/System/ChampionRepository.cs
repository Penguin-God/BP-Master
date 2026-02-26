using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChampionRepository : MonoBehaviour
{
    ChampionSO[] allChampion;
    public IReadOnlyList<ChampionSO> AllChampion => allChampion;
    public IEnumerable<int> AllId => allChampion.Select(x => x.Id);
    public Dictionary<int, string> NameCatalog { get; private set; }
    void Awake()
    {
        allChampion = LoadAllChampions();
        NameCatalog = allChampion.ToDictionary(x => x.Id, x => x.ChampionName);
    }
    public ChampionCatalog GetCatalog() => new ChampionCatalog(AllChampion.Select(x => x.CreateChampion()));
    public ChampionSO GetChampionData(int id) => allChampion.First(x =>  x.Id == id);
    public string GetChampionName(int id) => allChampion.First(x => x.Id == id).ChampionName;
    ChampionSO[] LoadAllChampions() => Resources.LoadAll<ChampionSO>("SO/Champions");
}


public static class ChampionDataLoder
{
    static ChampionSO[] _allChampions;
    public static IReadOnlyList<ChampionSO> AllChampions
    {
        get
        {
            _allChampions ??= Resources.LoadAll<ChampionSO>("SO/Champions");
            return _allChampions;
        }
    }

    public static IEnumerable<int> AllId => AllChampions.Select(x => x.Id);
    public static ChampionCatalog GetCatalog() => new ChampionCatalog(AllChampions.Select(x => x.CreateChampion()));
    public static ChampionSO GetChampionData(int id) => AllChampions.First(x => x.Id == id);
    public static string GetChampionName(int id) => GetChampionData(id).ChampionName;
}