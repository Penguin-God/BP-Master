
public class AI_BanPickAgent
{
    readonly Team Team;
    readonly GameBanPickStorage storage;
    readonly IBanSelector banSelector;
    readonly IPickSelector pickSelector;

    public AI_BanPickAgent(Team team, GameBanPickStorage storage, IBanSelector banSelector, IPickSelector pickSelector)
    {
        Team = team;
        this.storage = storage;
        this.banSelector = banSelector;
        this.pickSelector = pickSelector;
    }


    void Select(Team team, GamePhase phase, int id)
    {
        if (team != Team) return;
        storage.SaveSelect(new GameFlowData(phase, Team), id);
    }

    public void Ban(Team team) => Select(team, GamePhase.Ban, banSelector.Ban(storage.SelectableIds));
    public void Pick(Team team) => Select(team, GamePhase.Pick, pickSelector.Pick(storage.SelectableIds));
}
