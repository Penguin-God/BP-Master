using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] PlayerSO[] playerDatas;
    [SerializeField] ChampionManager championManager;
    HashSet<Player> players;
    public IReadOnlyList<Player> Players => players.ToArray();

    public void IncreasedMastery(int currentClickPlayer, ChampionSO championSO)
    {
        GetPlayer(currentClickPlayer).AddMastery(championSO);
    }

    void Awake()
    {
        players = playerDatas
                    .Select(x => new Player(x))
                    .ToHashSet();
    }

    Player GetPlayer(int id) => players.FirstOrDefault(x => x.Id == id);
}
