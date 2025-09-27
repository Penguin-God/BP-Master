using System.Collections.Generic;

public class TeamMasteryApplier
{
    readonly SlotStatusChanger statusChanger;
    public TeamMasteryApplier(SlotStatusChanger statusChanger) => this.statusChanger = statusChanger;

    public void Apply(IEnumerable<PickSlotData> datas)
    {
        foreach (var data in datas)
        {
            int level = data.GetActiveMastery();
            statusChanger.ChangeStat(data.Slot, stat => new MasteryApplier().ApplyMastery(stat, level));
        }
    }

    public void Apply(SlotStorage<ProGamer> gamers, SlotStorage<int> ids)
    {
        foreach (var slot in gamers.GetAllSlotDatas())
        {
            int level = gamers.GetSlot(slot).GetMastery(ids.GetSlot(slot));
            statusChanger.ChangeStat(slot, stat => new MasteryApplier().ApplyMastery(stat, level));
        }
    }
}
