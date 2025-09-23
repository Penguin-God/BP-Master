using System;

public class SelectFacade
{
    readonly ChampionCatalog championCatalog;
    readonly SlotStorage<ChampionStatus> statuses = new();

    public event Action<int> OnChampionSelected;

    public SelectFacade(ChampionCatalog championCatalog) => this.championCatalog = championCatalog;

    public void Pick(Team team, int championId)
    {
        var status = new ChampionStatus(championCatalog.GetChampion(championId).StatData);
        statuses.AddSlot(team, status);
    }

    public ChampionStatus GetStatus(SlotData slot) => statuses.GetSlot(slot);
}
