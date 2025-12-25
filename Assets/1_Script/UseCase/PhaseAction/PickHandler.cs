
public class PickHandler
{
    readonly ChampionCatalog championCatalog;
    readonly PickSlotFacade pickSlotFacade;

    public PickHandler(ChampionCatalog championCatalog, PickSlotFacade pickSlotFacade)
    {
        this.championCatalog = championCatalog;
        this.pickSlotFacade = pickSlotFacade;
    }

    public void Pick(Team team, int id)
    {
        var champion = championCatalog.GetChampion(id);
        pickSlotFacade.Add(team, champion);
    }
}
