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
}
