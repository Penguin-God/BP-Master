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
        foreach (var team in new[] { Team.Blue, Team.Red })
        {
            Enumerable.Range(0, slotCount)
                .ToList()
                .ForEach(_ => AddSlot(team, value));
        }
    }

    public void AddSlot(Team team, T value) => slots.Add(new SlotData(team, indexr.AllocateIndex(team)), value);
    public void AddSlots(Team team, IEnumerable<T> items)
    {
        foreach (var item in items)
            AddSlot(team, item);
    }
    public void ChangeSlot(SlotData slot, T value) => slots[slot] = value;
    public T GetSlot(SlotData slot) => slots[slot];

    public IEnumerable<T> GetTeam(Team team) 
        => slots
            .Where(kv => kv.Key.Team == team)
            .OrderBy(kv => kv.Key.Index)
            .Select(kv => kv.Value);
}
