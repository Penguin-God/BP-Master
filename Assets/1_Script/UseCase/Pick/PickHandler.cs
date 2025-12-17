
public class PickHandler
{
    readonly ChampionCatalog championCatalog;
    readonly PickSlotFacade pickSlotFacade;
    readonly TraitFactory traitFactory;
    readonly MasteryCollection masteryCollection;

    public PickHandler(ChampionCatalog championCatalog, PickSlotFacade pickSlotFacade, TraitFactory traitFactory, MasteryCollection masteryCollection)
    {
        this.championCatalog = championCatalog;
        this.pickSlotFacade = pickSlotFacade;
        this.traitFactory = traitFactory;
        this.masteryCollection = masteryCollection;
    }

    public void Pick(Team team, int id)
    {
        var champion = championCatalog.GetChampion(id);
        pickSlotFacade.Add(team, champion);
        traitFactory.Create(team, champion.Status.TraitType).Do();

        if (masteryCollection.HasMastery(id))
            new MasteryApplier().ApplyStatChange(champion.Status, masteryCollection.GetMasteryLevel(id));
    }
}
