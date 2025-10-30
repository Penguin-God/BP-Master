
public class AI_SelectAgent
{
    readonly Team Team;
    readonly PhaseManager phaseManager;
    readonly GameBanPickStorage storage;
    readonly IBanSelector banSelector;
    readonly IPickSelector pickSelector;

    public AI_SelectAgent(Team team, PhaseManager phaseManager, GameBanPickStorage storage, IBanSelector banSelector, IPickSelector pickSelector)
    {
        Team = team;
        this.phaseManager = phaseManager;
        this.storage = storage;
        this.banSelector = banSelector;
        this.pickSelector = pickSelector;
    }


    void Select(Team team, SelectType selectType)
    {
        if (team != Team) return;
        if (selectType == SelectType.Ban) storage.SaveSelect(new SelectInfo(Team, selectType, banSelector.Ban(storage.SelectableIds)));
        else storage.SaveSelect(new SelectInfo(Team, selectType, pickSelector.Pick(storage.SelectableIds)));
        phaseManager.SubmitAction(Team);
    }

    public void Ban(Team team) => Select(team, SelectType.Ban);
    public void Pick(Team team) => Select(team, SelectType.Pick);
}
