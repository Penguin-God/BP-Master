using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanPickController : MonoBehaviour
{
    BanPickManager banPickManager;

    public event Action<SelectData> OnSelectedChampion = null;
    [SerializeField] DraftTurnSO banTurnSO;
    [SerializeField] DraftTurnSO pickTurnSO;
    public Dictionary<Team, IBanPickAgent> agentDict = new();
    void Awake()
    {
        banPickManager = new BanPickManager(banTurnSO, pickTurnSO);
    }

    BanPickUI GetUserAgent() => FindAnyObjectByType<BanPickUI>();

    public void ChioceTeam(Team team)
    {
        agentDict.Add(team, GetUserAgent());
        if(team == Team.Blue) agentDict.Add(Team.Red, GetUserAgent());
        else agentDict.Add(Team.Blue, GetUserAgent());
        StartCoroutine(Co_BanPick(Team.Blue));
    }

    void SelectChampion(ChampionSO champion)
    {
        // 실패시 다시 돌림
        if (banPickManager.TrySelect(champion, out var data) == false)
        {
            StartCoroutine(Co_BanPick(data.NextTeam));
            return;
        }

        OnSelectedChampion?.Invoke(data);
        if (data.NextPhase < BanPcikPhase.Swap)
            StartCoroutine(Co_BanPick(data.NextTeam));
    }

    IEnumerator Co_BanPick(Team team)
    {
        yield return agentDict[team].WaitSelect();
        SelectChampion(agentDict[team].SelectChampion());
    }
}
