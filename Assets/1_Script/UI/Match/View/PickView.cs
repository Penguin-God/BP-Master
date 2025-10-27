using UnityEngine;

public class PickView : MonoBehaviour
{
    [SerializeField] ChampionRepository championManager;
    [SerializeField] Transform blueSlotParent;
    [SerializeField] Transform redSlotParent;
    public SlotStorage<SlotView> slotViews = new();
    SlotStorage<StatChangeView> statChangeViews = new();

    [SerializeField] GamerRoster playerRoster;
    [SerializeField] ChampionView mainView;
    void Start()
    {
        slotViews.AddSlots(Team.Blue, blueSlotParent.GetComponentsInChildren<SlotView>());
        slotViews.AddSlots(Team.Red, redSlotParent.GetComponentsInChildren<SlotView>());

        statChangeViews.AddSlots(Team.Blue, blueSlotParent.GetComponentsInChildren<StatChangeView>());
        statChangeViews.AddSlots(Team.Red, redSlotParent.GetComponentsInChildren<StatChangeView>());
    }

    public void BindStatChangeEvent(SlotStorage<ChampionStatus> statuses)
    {
        foreach (var slot in statuses.GetAllSlotDatas())
            statuses.GetSlot(slot).OnStatChanged += (be, af) => statChangeViews.GetSlot(slot).ChangeStat(new StatChangeData(be, af));
    }

    public void ViewMastery()
    {
        foreach (var slot in slotViews.GetAllSlotDatas())
            slotViews.GetSlot(slot).Init(mainView, championManager, playerRoster.Rosters.GetSlot(slot));
    }

    public void PickChampion(SlotData pickSlot, int id) => slotViews.GetSlot(pickSlot).UpdateChampion(id);
}
