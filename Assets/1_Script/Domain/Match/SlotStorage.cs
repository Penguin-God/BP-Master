using System.Collections.Generic;

public class SlotStorage
{
    readonly TeamSlotIndexr indexr = new TeamSlotIndexr();
    readonly Dictionary<SlotData, Champion> slots = new();

    public void AddSlot(Team team, Champion champion) => slots.Add(new SlotData(team, indexr.AllocateIndex(team)), champion);

    public Champion GetSlot(SlotData slot) => slots[slot];
}
