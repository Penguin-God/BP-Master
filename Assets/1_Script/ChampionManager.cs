using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChampionManager : MonoBehaviour
{
    [SerializeField] ChampionSO[] allChampion;
    public IReadOnlyList<ChampionSO> AllChampion => allChampion;

    void Awake()
    {
        allChampion = LoadAllChampions();
    }

    // 모든 챔 데이터 다 들고있는데 못찾는건 말도 안되는 상황이라 First() 사용
    public ChampionSO GetChampionData(int id) => allChampion.First(x =>  x.Id == id);

    public static ChampionSO[] LoadAllChampions()
    {
        return Resources.LoadAll<ChampionSO>("SO/Champions");
    }
}
