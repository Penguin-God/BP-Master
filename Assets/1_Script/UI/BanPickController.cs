using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class BanPickController : MonoBehaviour
{
    PhaseManager phaseManager = new();
    GameSelectStorage championStorage = new();

    public event Action<SelectData> OnSelectedChampion = null;
    [SerializeField] DraftTurnSO banTurnSO;
    [SerializeField] DraftTurnSO pickTurnSO;
    public Dictionary<Team, SelectAgent> agentDict = new();

    SelectAgent GetUserAgent() => new SelectAgent(FindAnyObjectByType<BanPickUI>(), FindAnyObjectByType<BanPickUI>());

    public void ChioceTeam(Team team)
    {
        var ai = new AI_BanPickAgent(championStorage, FindAnyObjectByType<ChampionManager>().AllChampion.Select(x => x.Id));
        SelectAgent ai_agent = new SelectAgent(ai, ai);
        agentDict.Add(team, GetUserAgent());
        if(team == Team.Blue) agentDict.Add(Team.Red, ai_agent);
        else agentDict.Add(Team.Blue, ai_agent);

        StartCoroutine(Co_BanPick());
    }

    IEnumerator Co_BanPick()
    {
        // 이거 그냥 미리 정의 가능함
        yield return Co_SelectLoop(banTurnSO.Turns, BanPcikPhase.Ban);
        phaseManager.ChangePhase(BanPcikPhase.Pick);
        yield return Co_SelectLoop(pickTurnSO.Turns, BanPcikPhase.Pick);
        phaseManager.ChangePhase(BanPcikPhase.Swap);
        print(phaseManager.CurrentPhase);
    }

    IEnumerator Co_SelectLoop(IEnumerable<Team> teamSequnce, BanPcikPhase phase)
    {
        foreach (var item in teamSequnce)
            yield return Co_SelectValidChampion(item, phase);
    }

    IEnumerator Co_SelectValidChampion(Team team, BanPcikPhase phase)
    {
        while (true)
        {
            yield return agentDict[team].Co_SelectWait();
            int selectId = agentDict[team].SelectChampion();
            if (SelectCondition(selectId))
            {
                SelectChampion(selectId, phase, team);
                break;
            }
        }
    }

    void SelectChampion(int champion, BanPcikPhase phase, Team team)
    {
        championStorage.SaveSelectChampion(phase, team, champion);
        OnSelectedChampion?.Invoke(new SelectData(champion, new TurnInfo(team, phase), championStorage.GetStorage(phase).GetCount(team)));
    }

    bool SelectCondition(int id) => championStorage.SelectChampions.Contains(id) == false;
}