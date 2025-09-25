
public class PickTableRegistry
{
    readonly ChampionCatalog championCatalog;
    readonly SlotStorage<ChampionStatus> statuses = new();
    public SlotStorage<ChampionStatus> Statuses => statuses;

    SlotStorage<Champion> champions = new();
    public SlotStorage<Champion> Champions => champions;

    SlotStorage<int> ids = new();
    public SlotStorage<int> Ids => ids;

    public PickTableRegistry(ChampionCatalog championCatalog) => this.championCatalog = championCatalog;

    public void Pick(Team team, int championId)
    {
        var champion = championCatalog.GetChampion(championId);
        champions.AddSlot(team, champion);
        statuses.AddSlot(team, new ChampionStatus(champion.StatData));
        ids.AddSlot(team, championId);
    }

    public ChampionStatus GetStatus(SlotData slot) => statuses.GetSlot(slot);
    public Champion GetChampion(SlotData slot) => champions.GetSlot(slot);
}
