
public class MatchBinder
{
    public void BindStorageEvents(GameBanPickStorage storage, PickFacade pickFacade)
    {
        storage.OnPick += pickFacade.Pick;
    }

    public void BindTraitEvents(TraitController traitController, PhaseManager phaseManager)
    {
        traitController.OnTraitUsed += phaseManager.SubmitAction;
    }
}
