using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProGamerManager : MonoBehaviour
{
    [SerializeField] PlayerSO[] playerDatas;
    [SerializeField] ChampionManagerMono championManager;
    HashSet<ProGamer> players;
    public IReadOnlyList<ProGamer> Players => players.ToArray();

    public void IncreasedMastery(int currentClickPlayer, ChampionSO championSO)
    {
        GetPlayer(currentClickPlayer).AddMastery(championSO);
    }

    void Awake()
    {
        players = playerDatas
                    .Select(x => new ProGamer(x))
                    .ToHashSet();
    }

    ProGamer GetPlayer(int id) => players.FirstOrDefault(x => x.Id == id);
}
