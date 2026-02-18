using System;

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

    public void Pick(Team team, int id)
    {
        var champion = championCatalog.GetChampion(id);
        PickSlotFacade.Add(team, champion);
        eventDispatcher.RaisePick(champion, team);
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

    public void Pick(Team team, int id)
    {
        var champion = championCatalog.GetChampion(id);
        storage.Pick(team, id);
        PickSlotFacade.Add(team, champion);
        // BanPickEventDispatcher.RaisePick(new PickChampion(id, champion.Skill, champion.Status, team));
        BanPickEventDispatcher.RaisePick(champion, team);
    }
}
