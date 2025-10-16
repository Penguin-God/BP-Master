public class SlotSelectionState
{
    public SlotSelectionState() { }
    public bool IsSelect { get; private set; }
    SlotData? selectedSlot;
    public SlotData SelectedSlot => selectedSlot.Value;

    public void SelectSlot(SlotData slot)
    {
        selectedSlot = slot;
        IsSelect = true;
    }

    public void Use()
    {
        selectedSlot = null;
        IsSelect = false;
    }
}
