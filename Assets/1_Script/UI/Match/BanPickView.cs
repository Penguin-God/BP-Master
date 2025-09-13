using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BanPickView : MonoBehaviour
{
    [SerializeField] ChampionManagerMono championManager;
    [SerializeField] ChampionView[] bluePicks;
    [SerializeField] ChampionView[] redPicks;
    readonly Dictionary<Team, ChampionView[]> pickTextDict = new();

    [SerializeField] TextMeshProUGUI blueBan;
    [SerializeField] TextMeshProUGUI redBan;
    readonly Dictionary<Team, TextMeshProUGUI> banTextDict = new();


    [SerializeField] TextMeshProUGUI selectChampionTxt;
    readonly BanPickPersenter banPickPersenter = new BanPickPersenter();
    void Start()
    {
        pickTextDict.Add(Team.Blue, bluePicks);
        pickTextDict.Add(Team.Red, redPicks);

        banTextDict.Add(Team.Blue, blueBan);
        banTextDict.Add(Team.Red, redBan);
        blueBan.text = string.Empty;
        redBan.text = string.Empty;
    }

    public void UpdateSelectChampion(ChampionSO champion) => selectChampionTxt.text = champion.ChampionName;


    public void UpdateSelectView(GamePhase phase, Team team, ChampionSO champion)
    {
        if (phase == GamePhase.Pick) UpdatePickView(team, champion);
        else if(phase == GamePhase.Ban) UpdateBanView(team, champion);
    }

    public void UpdateAllPick(IReadOnlyDictionary<Team, IReadOnlyList<Champion>> data)
    {
        for (int i = 0; i < data[Team.Blue].Count; i++)
            pickTextDict[Team.Blue][i].UpdateStat(data[Team.Blue][i].StatData);

        for (int i = 0; i < data[Team.Red].Count; i++)
            pickTextDict[Team.Red][i].UpdateStat(data[Team.Red][i].StatData);
    }

    void UpdatePickView(Team team, ChampionSO champion)
    {
        var slot = banPickPersenter.GetNextSlot(team);
        pickTextDict[team][slot.Index].UpdateDisplay(champion);
    }

    void UpdateBanView(Team team, ChampionSO champion)
    {
        banTextDict[team].text += champion.ChampionName;
        banTextDict[team].text += '\n';
    }
}
