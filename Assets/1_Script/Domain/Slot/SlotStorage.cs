using System.Collections.Generic;
using System.Linq;

public class SlotStorage<T>
{
    readonly TeamSlotIndexr indexr = new TeamSlotIndexr();
    readonly Dictionary<SlotData, T> slots = new();
    
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

    public IEnumerable<T> GetAll() => new[] { Team.Blue, Team.Red }.SelectMany(team => GetTeam(team));

    public IEnumerable<SlotData> GetAllSlotDatas() => slots.Keys;
}
