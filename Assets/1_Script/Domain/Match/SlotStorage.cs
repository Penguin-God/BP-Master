using System.Collections.Generic;

public class SlotStorage
{
    private readonly Dictionary<SlotData, ChampionStatData> slots = new();

    public void AddSlot(SlotData slot, Champion champion)
    {
        slots[slot] = champion.StatData;
    }

    public ChampionStatData GetSlot(SlotData slot)
    {
        return slots[slot];
    }
}
