using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BanView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI blueBan;
    [SerializeField] TextMeshProUGUI redBan;
    [SerializeField] ChampionRepository championManager;

    readonly Dictionary<Team, TextMeshProUGUI> banTextDict = new();

    void Start()
    {
        banTextDict.Add(Team.Blue, blueBan);
        banTextDict.Add(Team.Red, redBan);
        blueBan.text = string.Empty;
        redBan.text = string.Empty;
    }

    public void UpdateBanList(Team team, int id)
    {
        banTextDict[team].text += championManager.GetChampionData(id).ChampionName;
        banTextDict[team].text += ", ";
    }

    public void HideBan()
    {
        blueBan.gameObject.SetActive(false);
        redBan.gameObject.SetActive(false);
    }
}
