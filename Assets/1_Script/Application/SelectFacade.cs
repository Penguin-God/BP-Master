


public class SelectFacade
{
    readonly ChampionCatalog championCatalog;
    readonly GameBanPickStorage selectStorage;
    readonly SlotStorage<ChampionStatus> statuses = new();

    public SelectFacade(ChampionCatalog championCatalog, GameBanPickStorage selectStorage)
    {
        this.championCatalog = championCatalog;
        this.selectStorage = selectStorage;
    }

    public bool Pick(Team team, int championId)
    {
        if (selectStorage.SaveSelect(new SelectInfo(team, SelectType.Pick, championId)) == false) return false;

        var status = new ChampionStatus(championCatalog.GetChampion(championId).StatData);
        statuses.AddSlot(team, status);

        return true;
    }

    public ChampionStatus GetStatus(SlotData slot) => statuses.GetSlot(slot);

    public Champion GetChampion(SlotData slotData)
    {
        int id = selectStorage.GetStorage(slotData.Team, SelectType.Pick)[slotData.Index];
        return championCatalog.GetChampion(id);
    }
}
