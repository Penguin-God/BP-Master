using UnityEngine;

public class SlotViewOrchestrator : MonoBehaviour
{
    [SerializeField] ChampionRepository championManager;
    [SerializeField] Transform blueSlotParent;
    [SerializeField] Transform redSlotParent;
    SlotStorage<SlotView> slotViews = new();
    SlotStorage<ChampionStatusTrackerView> trackerViewSlots = new();
    SlotStorage<ChampionStatus> statusSlots;

    [SerializeField] ChampionView mainView;
    void Start()
    {
        slotViews.AddSlots(Team.Blue, blueSlotParent.GetComponentsInChildren<SlotView>());
        slotViews.AddSlots(Team.Red, redSlotParent.GetComponentsInChildren<SlotView>());

        trackerViewSlots.AddSlots(Team.Blue, blueSlotParent.GetComponentsInChildren<ChampionStatusTrackerView>());
        trackerViewSlots.AddSlots(Team.Red, redSlotParent.GetComponentsInChildren<ChampionStatusTrackerView>());
    }

    public void InitSlotView(SlotStorage<ChampionStatus> statuses)
    {
        statusSlots = statuses;
        foreach (var slot in slotViews.GetAllSlotDatas())
            slotViews.GetSlot(slot).Init(mainView, championManager);
    }

    public void PickChampion(SlotData pickSlot, int id)
    {
        slotViews.GetSlot(pickSlot).UpdateChampion(statusSlots.GetSlot(pickSlot), id);
        trackerViewSlots.GetSlot(pickSlot).Init(statusSlots.GetSlot(pickSlot));
    }
}
