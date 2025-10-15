
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

    void Select(Team team, SelectType selectType)
    {
        if (team != Team) return;
        storage.SaveSelect(new SelectInfo(Team, selectType, selector.Ban(storage.SelectableIds)));
        phaseManager.SubmitAction(Team);
    }

    public void Ban(Team team) => Select(team, SelectType.Ban);
    public void Pick(Team team) => Select(team, SelectType.Pick);
}
