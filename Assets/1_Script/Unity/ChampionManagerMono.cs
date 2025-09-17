using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChampionManagerMono : MonoBehaviour
{
    [SerializeField] ChampionSO[] allChampion;
    public ChampionCatalog ChampionManager { get; private set; }
    
    public IReadOnlyList<ChampionSO> AllChampion => allChampion;
    public IReadOnlyList<int> AllId => allChampion.Select(x => x.Id).ToList();

    void Awake()
    {
        allChampion = LoadAllChampions();
        ChampionManager = new ChampionCatalog(allChampion.Select(x => x.CreateChampion()));
    }

    // 모든 챔 데이터 다 들고있는데 못찾는건 말도 안되는 상황이라 First() 사용
    public ChampionSO GetChampionData(int id) => allChampion.First(x =>  x.Id == id);
    public Champion GetChampion(int id) => ChampionManager.GetChampion(id);

    public static ChampionSO[] LoadAllChampions() => Resources.LoadAll<ChampionSO>("SO/Champions");
}
