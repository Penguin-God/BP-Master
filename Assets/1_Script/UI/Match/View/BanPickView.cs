using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BanPickView : MonoBehaviour
{
    [SerializeField] ChampionRepository championManager;
    [SerializeField] Transform blueSlotParent;
    [SerializeField] Transform redSlotParent;
    public SlotStorage<SlotView> slotViews = new();
    SlotStorage<StatChangeView> statChangeViews = new();

    [SerializeField] TextMeshProUGUI blueBan;
    [SerializeField] TextMeshProUGUI redBan;
    readonly Dictionary<Team, TextMeshProUGUI> banTextDict = new();
    [SerializeField] PlayerRoster playerRoster;

    readonly TeamSlotIndexr pickCursor = new TeamSlotIndexr();
    [SerializeField] ChampionView mainView;
    void Start()
    {
        slotViews.AddSlots(Team.Blue, blueSlotParent.GetComponentsInChildren<SlotView>());
        slotViews.AddSlots(Team.Red, redSlotParent.GetComponentsInChildren<SlotView>());

        foreach (var slot in slotViews.GetAllSlotDatas())
            slotViews.GetSlot(slot).Init(mainView, championManager, playerRoster.Rosters.GetSlot(slot));

        statChangeViews.AddSlots(Team.Blue, blueSlotParent.GetComponentsInChildren<StatChangeView>());
        statChangeViews.AddSlots(Team.Red, redSlotParent.GetComponentsInChildren<StatChangeView>());

        banTextDict.Add(Team.Blue, blueBan);
        banTextDict.Add(Team.Red, redBan);
        blueBan.text = string.Empty;
        redBan.text = string.Empty;
    }

    public void ChangeChampionStat(StatChangeData statChangeData) => statChangeViews.GetSlot(statChangeData.Slot).ChangeStat(statChangeData);

    public void UpdatePickView(Team team, int id) => slotViews.GetSlot(new SlotData(team, pickCursor.AllocateIndex(team))).UpdateChampion(championManager.GetChampionData(id).CreateChampion());
    // public void UpdatePickView(PickSlotData data) => slotViews.GetSlot(data.Slot).UpdateChampion(data.ChampId);
    public void UpdateBanView(Team team, int id)
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
