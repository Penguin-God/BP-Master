using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BanPickView : MonoBehaviour
{
    [SerializeField] ChampionRepository championManager;
    [SerializeField] Transform blueSlotParent;
    [SerializeField] Transform redSlotParent;
    SlotStorage<SlotView> slotViews = new();
    SlotStorage<ChampionView> pickViews = new();

    [SerializeField] TextMeshProUGUI blueBan;
    [SerializeField] TextMeshProUGUI redBan;
    readonly Dictionary<Team, TextMeshProUGUI> banTextDict = new();

    readonly TeamSlotIndexr pickCursor = new TeamSlotIndexr();
    [SerializeField] ChampionView mainView;
    void Start()
    {
        slotViews.AddSlots(Team.Blue, blueSlotParent.GetComponentsInChildren<SlotView>());
        slotViews.AddSlots(Team.Red, redSlotParent.GetComponentsInChildren<SlotView>());

        foreach (var item in slotViews.GetAll())
            item.Init(mainView, championManager);

        pickViews.AddSlots(Team.Blue, blueSlotParent.GetComponentsInChildren<ChampionView>());
        pickViews.AddSlots(Team.Red, redSlotParent.GetComponentsInChildren<ChampionView>());

        banTextDict.Add(Team.Blue, blueBan);
        banTextDict.Add(Team.Red, redBan);
        blueBan.text = string.Empty;
        redBan.text = string.Empty;
    }

    public void UpdateSelectView(GamePhase phase, Team team, ChampionSO champion)
    {
        if (phase == GamePhase.Pick) UpdatePickView(team, champion.CreateChampion());
        else if(phase == GamePhase.Ban) UpdateBanView(team, champion);
    }

    public void ChangeChampionStat(StatChangeData statChangeData) => slotViews.GetSlot(statChangeData.Slot).ChangeStat(statChangeData);

    void UpdatePickView(Team team, Champion champion) => slotViews.GetSlot(new SlotData(team, pickCursor.AllocateIndex(team))).UpdateChampion(champion);

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
