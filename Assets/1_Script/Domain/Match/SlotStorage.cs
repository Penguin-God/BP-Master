using System.Collections.Generic;

public class SlotStorage
{
    readonly Dictionary<SlotData, Champion> slots = new();

    public void AddSlot(SlotData slot, Champion champion) => slots[slot] = champion;

    public Champion GetSlot(SlotData slot) => slots[slot];
}
