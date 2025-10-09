
public class AI_SelectAgent
{
    readonly Team Team;
    readonly PhaseManager phaseManager;
    readonly GameBanPickStorage storage;
    readonly IAI_Selector selector;

    public AI_SelectAgent(Team team, PhaseManager phaseManager, GameBanPickStorage storage, IAI_Selector selector)
    {
        Team = team;
        this.phaseManager = phaseManager;
        this.storage = storage;
        this.selector = selector;
    }

    public void Select(Team team)
    {
        if (team != Team) return;

        if (phaseManager.CurrentFlow.Phase == GamePhase.Ban)
            storage.SaveSelect(new SelectInfo(Team, SelectType.Ban, selector.Ban(storage.SelectableIds)));
        else if(phaseManager.CurrentFlow.Phase == GamePhase.Pick)
            storage.SaveSelect(new SelectInfo(Team, SelectType.Pick, selector.Pick(storage.SelectableIds)));
        phaseManager.SubmitAction(Team);
    }
}
