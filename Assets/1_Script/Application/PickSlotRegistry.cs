using System;
using System.Collections.Generic;

public class PickSlotData
{
    public readonly SlotData Slot;
    readonly ProGamer gamer;
    public int ChampId { get; private set; }

    public PickSlotData(SlotData slotData, ProGamer gamer)
    {
        Slot = slotData;
        this.gamer = gamer;
    }

    public void Pick(int id) => ChampId = id;
    public int GetActiveMastery() => gamer.GetMastery(ChampId);
}


public class PickSlotRegistry
{
    List<PickSlotData> pickSlotDatas = new();
    public IEnumerable<PickSlotData> PickSlotDatas => pickSlotDatas;
    public event Action<PickSlotData> OnSlotPick;

    readonly SlotStorage<ProGamer> gamers;
    public PickSlotRegistry(SlotStorage<ProGamer> gamers) => this.gamers = gamers;
    TeamSlotIndexr slotIndexr = new();

    public void Pick(Team team, int id)
    {
        int index = slotIndexr.AllocateIndex(team);
        var slot = new SlotData(team, index);
        var data = new PickSlotData(slot, gamers.GetSlot(slot));
        data.Pick(id);
        pickSlotDatas.Add(data);
        OnSlotPick?.Invoke(data);
    }
}
