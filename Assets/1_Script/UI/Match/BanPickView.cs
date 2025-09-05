using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BanPickView : MonoBehaviour
{
    [SerializeField] ChampionManagerMono championManager;
    [SerializeField] TextMeshProUGUI[] bluePicks;
    [SerializeField] TextMeshProUGUI[] redPicks;
    readonly Dictionary<Team, TextMeshProUGUI[]> pickTextDict = new();

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
    }

    public void UpdateSelectChampion(ChampionSO champion) => selectChampionTxt.text = champion.ChampionName;

    int blueIndex;
    int redIndex;
    public void UpdateSelectView(SelectType selectType, Team team, int id)
    {
        if (selectType == SelectType.Pick) UpdatePickView(team, id);
        else if(selectType == SelectType.Ban) UpdateBanView(team, id);
    }

    void UpdatePickView(Team team, int id)
    {
        if(team == Team.Red)
        {
            pickTextDict[team][redIndex].text = championManager.GetChampionName(id);
            redIndex++;
        }
        else if(team == Team.Blue)
        {
            pickTextDict[team][blueIndex].text = championManager.GetChampionName(id);
            blueIndex++;
        }
    }

    
    void UpdateBanView(Team team, int id)
    {
        banTextDict[team].text += championManager.GetChampionName(id);
    }
}
