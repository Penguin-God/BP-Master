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
    public ChampionSO GetChampionData(int id) => allChampion.First(x =>  x.Id == id);
    ChampionSO[] LoadAllChampions() => Resources.LoadAll<ChampionSO>("SO/Champions");
}
