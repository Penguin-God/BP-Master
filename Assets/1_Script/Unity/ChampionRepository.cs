using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChampionRepository : MonoBehaviour
{
    ChampionSO[] allChampion;
    public IReadOnlyList<ChampionSO> AllChampion => allChampion;
    public ChampionCatalog Catalog { get; private set; }
    // 이름만 주는 딕셔너리
    void Awake()
    {
        allChampion = LoadAllChampions();
        Catalog = new ChampionCatalog(allChampion.Select(x => x.CreateChampion()));
    }
    public ChampionSO GetChampionData(int id) => allChampion.First(x =>  x.Id == id);
    ChampionSO[] LoadAllChampions() => Resources.LoadAll<ChampionSO>("SO/Champions");
}
