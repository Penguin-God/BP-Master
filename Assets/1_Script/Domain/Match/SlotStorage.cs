using System.Collections.Generic;

public class SlotStorage
{
    private readonly Dictionary<ChampionSlot, ChampionStatData> slots = new();

    public void AddSlot(ChampionSlot slot, Champion champion)
    {
        slots[slot] = champion.StatData;
    }

    public ChampionStatData GetSlot(ChampionSlot slot)
    {
        return slots[slot];
    }
}
