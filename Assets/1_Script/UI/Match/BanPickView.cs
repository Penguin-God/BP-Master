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


    
    readonly TeamSlotIndexr pickCursor = new TeamSlotIndexr();
    void Start()
    {
        pickTextDict.Add(Team.Blue, bluePicks);
        pickTextDict.Add(Team.Red, redPicks);

        banTextDict.Add(Team.Blue, blueBan);
        banTextDict.Add(Team.Red, redBan);
        blueBan.text = string.Empty;
        redBan.text = string.Empty;
    }

    public void UpdateSelectView(GamePhase phase, Team team, ChampionSO champion)
    {
        if (phase == GamePhase.Pick) UpdatePickView(team, champion);
        else if(phase == GamePhase.Ban) UpdateBanView(team, champion);
    }

    public void UpdateAllPick(IReadOnlyDictionary<Team, IReadOnlyList<Champion>> data)
    {
        for (int i = 0; i < data[Team.Blue].Count; i++)
            pickTextDict[Team.Blue][i].UpdateStat(data[Team.Blue][i]);

        for (int i = 0; i < data[Team.Red].Count; i++)
            pickTextDict[Team.Red][i].UpdateStat(data[Team.Red][i]);
    }

    void UpdatePickView(Team team, ChampionSO champion) => pickTextDict[team][pickCursor.GetNextIndex(team)].UpdateStat(champion.CreateChampion());

    void UpdateBanView(Team team, ChampionSO champion)
    {
        banTextDict[team].text += champion.ChampionName;
        banTextDict[team].text += ", ";
    }

    public void HideBan()
    {
        blueBan.gameObject.SetActive(false);
        redBan.gameObject.SetActive(false);
    }
}
