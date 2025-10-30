using UnityEngine;

public class SlotViewOrchestrator : MonoBehaviour
{
    [SerializeField] ChampionRepository championManager;
    [SerializeField] Transform blueSlotParent;
    [SerializeField] Transform redSlotParent;
    SlotStorage<SlotView> slotViews = new();
    SlotStorage<ChampionStatusTrackerView> trackerViewSlots = new();

    [SerializeField] GamerRoster playerRoster;
    [SerializeField] ChampionView mainView;
    void Start()
    {
        slotViews.AddSlots(Team.Blue, blueSlotParent.GetComponentsInChildren<SlotView>());
        slotViews.AddSlots(Team.Red, redSlotParent.GetComponentsInChildren<SlotView>());

        trackerViewSlots.AddSlots(Team.Blue, blueSlotParent.GetComponentsInChildren<ChampionStatusTrackerView>());
        trackerViewSlots.AddSlots(Team.Red, redSlotParent.GetComponentsInChildren<ChampionStatusTrackerView>());
    }

    public void InitTrackerViewSlots(SlotStorage<ChampionStatus> statuses)
    {
        foreach (var slot in statuses.GetAllSlotDatas())
            trackerViewSlots.GetSlot(slot).Init(statuses.GetSlot(slot));
    }

    public void InitSlotView()
    {
        foreach (var slot in slotViews.GetAllSlotDatas())
            slotViews.GetSlot(slot).Init(mainView, championManager, playerRoster.Rosters.GetSlot(slot));
    }

    public void PickChampion(SlotData pickSlot, int id) => slotViews.GetSlot(pickSlot).UpdateChampion(id);
}
