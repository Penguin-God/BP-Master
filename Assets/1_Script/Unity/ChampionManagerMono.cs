using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChampionManagerMono : MonoBehaviour
{
    [SerializeField] ChampionSO[] allChampion;
    
    public IReadOnlyList<ChampionSO> AllChampion => allChampion;

    void Awake() => allChampion = LoadAllChampions();
    public ChampionSO GetChampionData(int id) => allChampion.First(x =>  x.Id == id);

    public static ChampionSO[] LoadAllChampions() => Resources.LoadAll<ChampionSO>("SO/Champions");
}
