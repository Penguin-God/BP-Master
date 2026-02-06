using System;

public class PickHandler
{
    readonly ChampionCatalog championCatalog;
    public readonly PickSlotFacade PickSlotFacade = new();
    readonly PhaseActionEventDispatcher eventDispatcher;
    public PickHandler(ChampionCatalog championCatalog, PhaseActionEventDispatcher eventDispatcher)
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
    public BanPickHandler(ChampionCatalog championCatalog, BanPickStorage storage)
    {
        this.championCatalog = championCatalog;
        this.storage = storage;
    }

    public event Action<PickChampion> OnChampionPick;

    public void Pick(Team team, int id)
    {
        var champion = championCatalog.GetChampion(id);
        storage.Pick(team, id);
        PickSlotFacade.Add(team, champion);
        OnChampionPick?.Invoke(new PickChampion(id, champion.Skill, champion.Status, team));
    }
}
