using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProGamerManager : MonoBehaviour
{
    [SerializeField] PlayerSO[] playerDatas;
    [SerializeField] ChampionManagerMono championManager;

    Dictionary<int, ProGamer> proGamerMap;
    HashSet<ProGamerDel> players;
    public IReadOnlyList<ProGamerDel> Players => players.ToArray();

    public void IncreasedMastery(int currentClickPlayer, ChampionSO championSO)
    {
        GetPlayer(currentClickPlayer).AddMastery(championSO);
    }

    public void IncreasedMastery(int gamer, int champId) => proGamerMap[gamer].AddMastery(champId);
    public int GetMastery(int gamer, int champId) => proGamerMap[gamer].GetMastery(champId);

    void Awake()
    {
        proGamerMap = playerDatas.ToDictionary(x => x.Id, x => x.CreateGamer());

        players = playerDatas
                    .Select(x => new ProGamerDel(x))
                    .ToHashSet();
    }

    ProGamerDel GetPlayer(int id) => players.FirstOrDefault(x => x.Id == id);
}
