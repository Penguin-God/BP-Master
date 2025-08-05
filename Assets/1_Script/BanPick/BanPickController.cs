using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BanPickController : MonoBehaviour
{
    BanPickManager banPickManager;
    ChampionSelectStorage championStorage;

    public event Action<SelectData> OnSelectedChampion = null;
    [SerializeField] DraftTurnSO banTurnSO;
    [SerializeField] DraftTurnSO pickTurnSO;
    public Dictionary<Team, IBanPickAgent> agentDict = new();

    void Awake()
    {
        banPickManager = new BanPickManager(banTurnSO, pickTurnSO);
    }

    void Start()
    {
        championStorage = new ChampionSelectStorage(FindAnyObjectByType<ChampionManager>().AllChampion);
    }

    BanPickUI GetUserAgent() => FindAnyObjectByType<BanPickUI>();

    public void ChioceTeam(Team team)
    {
        agentDict.Add(team, GetUserAgent());
        if(team == Team.Blue) agentDict.Add(Team.Red, new AI_BanPickAgent(championStorage));
        else agentDict.Add(Team.Blue, new AI_BanPickAgent(championStorage));
        StartCoroutine(Co_BanPick(Team.Blue));
    }

    void SelectChampion(ChampionSO champion)
    {
        // 실패시 다시 돌림
        if (banPickManager.TrySelect(champion, out var data) == false)
        {
            StartCoroutine(Co_BanPick(data.NextTurn.Team));
            return;
        }

        championStorage.SaveSelectChampion(champion);
        OnSelectedChampion?.Invoke(data);
        if (data.NextTurn.Phase < BanPcikPhase.Swap)
            StartCoroutine(Co_BanPick(data.NextTurn.Team));
        else
            print("Done");
    }

    IEnumerator Co_BanPick(Team team)
    {
        yield return agentDict[team].WaitSelect();
        SelectChampion(agentDict[team].SelectChampion());
    }
}

public class ChampionSelectStorage
{
    HashSet<ChampionSO> selectableChampions;
    public IReadOnlyList<ChampionSO> SelectableChampions => selectableChampions.ToArray();
    public ChampionSelectStorage(IReadOnlyList<ChampionSO> champions) => selectableChampions = new HashSet<ChampionSO>(champions);

    public void SaveSelectChampion(ChampionSO champion)
    {
        selectableChampions.Remove(champion);
    }
}