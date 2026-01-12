
public class AI_BanPickAgent
{
    readonly Team Team;
    readonly GameBanPickStorage storage;
    readonly IChampionSelector banSelector;
    readonly IChampionSelector pickSelector;

    public AI_BanPickAgent(Team team, GameBanPickStorage storage, IChampionSelector banSelector, IChampionSelector pickSelector)
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

    public void Ban(Team team) => Select(team, GamePhase.Ban, banSelector.Select(storage.SelectableIds));
    public void Pick(Team team) => Select(team, GamePhase.Pick, pickSelector.Select(storage.SelectableIds));
}
