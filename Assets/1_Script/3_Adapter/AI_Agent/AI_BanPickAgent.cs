
public class AI_BanPickAgent
{
    readonly Team Team;
    readonly BanPickStorage storage;
    readonly IChampionSelector banSelector;
    readonly IChampionSelector pickSelector;
    readonly BanPickHandler banPickHandler;
    public AI_BanPickAgent(Team team, BanPickStorage storage, IChampionSelector banSelector, IChampionSelector pickSelector, BanPickHandler banPickHandler)
    {
        Team = team;
        this.storage = storage;
        this.banSelector = banSelector;
        this.pickSelector = pickSelector;
        this.banPickHandler = banPickHandler;
    }


    void Select(Team team, GamePhase phase, int id)
    {
        if (team != Team) return;
        banPickHandler.SaveSelect(new GameFlowData(phase, Team), id);
    }

    public void Ban(Team team) => Select(team, GamePhase.Ban, banSelector.Select(storage.SelectableIds));
    public void Pick(Team team) => Select(team, GamePhase.Pick, pickSelector.Select(storage.SelectableIds));
}
