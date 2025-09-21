using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChampionRepository : MonoBehaviour
{
    ChampionSO[] allChampion;
    public IReadOnlyList<ChampionSO> AllChampion => allChampion;
    public ChampionCatalog Catalog { get; private set; }

    void Awake()
    {
        allChampion = LoadAllChampions();
        Catalog = new ChampionCatalog(allChampion.Select(x => x.CreateChampion()));
    }
    public ChampionSO GetChampionData(int id) => allChampion.First(x =>  x.Id == id);
    ChampionSO[] LoadAllChampions() => Resources.LoadAll<ChampionSO>("SO/Champions");
}
