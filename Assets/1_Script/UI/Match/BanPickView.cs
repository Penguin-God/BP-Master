using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BanPickView : MonoBehaviour
{
    [SerializeField] ChampionManager championManager;
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

    public void UpdatePickChampions(SelectData data)
    {
        if (data.CurrentTurn.Phase == GamePhase.Pick)
            pickTextDict[data.CurrentTurn.Team][data.Count - 1].text = championManager.GetChampionName(data.Id);
    }

    public void UpdateBanView(Team team, int id)
    {
        banTextDict[team].text += championManager.GetChampionName(id);
    }
}
