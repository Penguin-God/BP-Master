using System.Collections.Generic;
using System.Linq;

public class SlotStorage<T>
{
    readonly TeamSlotIndexr indexr = new TeamSlotIndexr();
    readonly Dictionary<SlotData, T> slots = new();

    public SlotStorage() { }

    // 슬롯 수와 기본값으로 초기화 (Blue 팀 기준)
    public SlotStorage(int slotCount, T value)
    {
        Enumerable.Range(0, slotCount)
            .ToList()
            .ForEach(_ => AddSlot(Team.Blue, value));
    }

    public void AddSlot(Team team, T champion) => slots.Add(new SlotData(team, indexr.AllocateIndex(team)), champion);

    public T GetSlot(SlotData slot) => slots[slot];

    public IEnumerable<T> GetTeam(Team team) 
        => slots
            .Where(kv => kv.Key.Team == team)
            .OrderBy(kv => kv.Key.Index)
            .Select(kv => kv.Value);
}
