using UnityEngine;

public class SlotViewOrchestrator : MonoBehaviour
{
    [SerializeField] Transform blueSlotParent;
    [SerializeField] Transform redSlotParent;
    SlotStorage<SlotView> slotViews = new();
    SlotStorage<ChampionStatus> statusSlots;

    void Start()
    {
        if (blueSlotParent != null)
            slotViews.AddSlots(Team.Blue, blueSlotParent.GetComponentsInChildren<SlotView>());
        if (redSlotParent != null)
            slotViews.AddSlots(Team.Red, redSlotParent.GetComponentsInChildren<SlotView>());
    }

    public void InitSlotView(SlotStorage<ChampionStatus> statuses)
    {
        statusSlots = statuses;
    }

    public SlotView GetSlotView(SlotData slotData) => slotViews.GetSlot(slotData);

    public void PickChampion(SlotData pickSlot, int id)
    {
        var pickSlotStatus = statusSlots.GetSlot(pickSlot);
        slotViews.GetSlot(pickSlot).PickCard(pickSlotStatus, id);
    }
}
