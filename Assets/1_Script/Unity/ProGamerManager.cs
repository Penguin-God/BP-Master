using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProGamerManager : MonoBehaviour
{
    [SerializeField] ProGamerSO[] playerDatas;
    [SerializeField] ChampionManagerMono championManager;

    HashSet<ProGamerDel> players;
    public IReadOnlyList<ProGamerDel> Players => players.ToArray();

    public void IncreasedMastery(int currentClickPlayer, ChampionSO championSO)
    {
        GetPlayer(currentClickPlayer).AddMastery(championSO);
    }


    void Awake()
    {
        players = playerDatas
                    .Select(x => new ProGamerDel(x))
                    .ToHashSet();
    }

    ProGamerDel GetPlayer(int id) => players.FirstOrDefault(x => x.Id == id);
}
