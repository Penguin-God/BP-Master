using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BanPickView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI[] bluePicks;
    [SerializeField] TextMeshProUGUI[] redPicks;
    readonly Dictionary<Team, TextMeshProUGUI[]> pickTextDict = new();

    [SerializeField] TextMeshProUGUI selectChampionTxt;

    void Start()
    {
        pickTextDict.Add(Team.Blue, bluePicks);
        pickTextDict.Add(Team.Red, redPicks);
    }

    public void UpdateSelectChampion(ChampionSO champion) => selectChampionTxt.text = champion.ChampionName;

    public void UpdatePickChampions(SelectData data)
    {
        if (data.BanPcikPhase == BanPcikPhase.Pick)
            pickTextDict[data.Team][data.Count - 1].text = data.Champion.ChampionName;
    }
}
