using System;

public class PickStatusChanger
{
    readonly SlotStorage<ChampionStatus> statusTable;
    public event Action<StatChangeData> OnStatChanged;

    public PickStatusChanger(SlotStorage<ChampionStatus> statusTable) => this.statusTable = statusTable;

    public void ChangeStat(SlotData slot, ChampionStatData newStat)
    {
        var status = statusTable.GetSlot(slot);
        var beforeStat = status.StatData;

        if (beforeStat.Equals(newStat)) return;

        status.ChangeStat(newStat);
        OnStatChanged?.Invoke(new StatChangeData(slot, beforeStat, newStat));
    }
}
