using System.Collections.Generic;
using System.Linq;

public class SlotStorage
{
    readonly TeamSlotIndexr indexr = new TeamSlotIndexr();
    readonly Dictionary<SlotData, Champion> slots = new();

    public void AddSlot(Team team, Champion champion) => slots.Add(new SlotData(team, indexr.AllocateIndex(team)), champion);

    public Champion GetSlot(SlotData slot) => slots[slot];

    public IEnumerable<Champion> GetTeam(Team team) 
        => slots
            .Where(kv => kv.Key.Team == team)
            .OrderBy(kv => kv.Key.Index)
            .Select(kv => kv.Value);
}
