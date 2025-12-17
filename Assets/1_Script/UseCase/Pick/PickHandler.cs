
public class PickHandler
{
    var champion = champManager.GetChampionData(id).CreateChampion();
    pickSlotFacade.Add(slotData.Team, champion);
        new TraitFactory(matchConfig.TraitConfig, pickSlotFacade.StatusSlots).Create(slotData.Team, champion.Status.TraitType).Do();

    MasteryCollection masteryCollection = masteryGenerator.GetTeamMasteryManager(slotData.Team);
        if (masteryCollection.HasMastery(id))
            new MasteryApplier().ApplyStatChange(champion.Status, masteryCollection.GetMasteryLevel(id));
}
