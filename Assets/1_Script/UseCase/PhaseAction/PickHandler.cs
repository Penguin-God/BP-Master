
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
        // if(gameBanPickStorage.CanSelected(id) == false) throw new Exception($"픽할 수 없는 ID : {id}");
        var champion = championCatalog.GetChampion(id);
        PickSlotFacade.Add(team, champion);
        eventDispatcher.RaisePick(champion);
    }
}
