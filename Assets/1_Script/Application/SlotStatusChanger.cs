using System;

public class SlotStatusChanger
{
    readonly SlotStorage<ChampionStatus> statusTable;
    public event Action<StatChangeData> OnStatChanged;

    public SlotStatusChanger(SlotStorage<ChampionStatus> statusTable) => this.statusTable = statusTable;

    public void ChangeStat(SlotData slot, ChampionStatData newStat)
    {
        var beforeStat = GetStat(slot);

        if (beforeStat.Equals(newStat)) return;

        statusTable.GetSlot(slot).ChangeStat(newStat);
        OnStatChanged?.Invoke(new StatChangeData(slot, beforeStat, newStat));
    }

    public void ChangeStat(SlotData slot, Func<ChampionStatData, ChampionStatData> newStat) => ChangeStat(slot, newStat(GetStat(slot)));
    ChampionStatData GetStat(SlotData slot) => statusTable.GetSlot(slot).Stat;
}
