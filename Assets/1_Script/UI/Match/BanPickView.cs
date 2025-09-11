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

    void Start()
    {
        pickTextDict.Add(Team.Blue, bluePicks);
        pickTextDict.Add(Team.Red, redPicks);

        banTextDict.Add(Team.Blue, blueBan);
        banTextDict.Add(Team.Red, redBan);
        blueBan.text = string.Empty;
        redBan.text = string.Empty;

    }

    // 나중에 전부 깔기
    public void UpdateSelectChampion(ChampionSO champion) => selectChampionTxt.text = champion.ChampionName;

    int blueIndex;
    int redIndex;
    public void UpdateSelectView(GamePhase phase, Team team, ChampionSO champion)
    {
        if (phase == GamePhase.Pick) UpdatePickView(team, champion);
        else if(phase == GamePhase.Ban) UpdateBanView(team, champion);
    }

    void UpdatePickView(Team team, ChampionSO champion)
    {
        if(team == Team.Red)
        {
            pickTextDict[team][redIndex].UpdateDisplay(champion);
            redIndex++;
        }
        else if(team == Team.Blue)
        {
            pickTextDict[team][blueIndex].UpdateDisplay(champion);
            blueIndex++;
        }
    }

    
    void UpdateBanView(Team team, ChampionSO champion)
    {
        banTextDict[team].text += champion.ChampionName;
        banTextDict[team].text += '\n';
    }
}
