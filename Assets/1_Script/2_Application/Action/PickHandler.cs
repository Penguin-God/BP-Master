
public class PickHandler
{
    readonly ChampionCatalog championCatalog;
    public readonly PickSlotFacade PickSlotFacade = new();
    readonly BanPickEventDispatcher eventDispatcher;
    public PickHandler(ChampionCatalog championCatalog, BanPickEventDispatcher eventDispatcher)
    {
        this.championCatalog = championCatalog;
        this.eventDispatcher = eventDispatcher;
    }

    public void Pick(SlotData slotData, int id)
    {
        var champion = championCatalog.GetChampion(id);
        PickSlotFacade.Add(slotData.Team, champion);
        eventDispatcher.RaisePick(champion, slotData);
    }
}

public class BanPickHandler
{
    readonly ChampionCatalog championCatalog;
    public readonly PickSlotFacade PickSlotFacade = new();
    readonly BanPickStorage storage;
    public readonly BanPickEventDispatcher BanPickEventDispatcher = new();

    public BanPickHandler(ChampionCatalog championCatalog, BanPickStorage storage)
    {
        this.championCatalog = championCatalog;
        this.storage = storage;
    }

    public void Pick(SlotData slotData, int id)
    {
        var champion = championCatalog.GetChampion(id);
        storage.Pick(slotData.Team, id);
        PickSlotFacade.Add(slotData.Team, champion);
        BanPickEventDispatcher.RaisePick(champion, slotData);
    }
}
