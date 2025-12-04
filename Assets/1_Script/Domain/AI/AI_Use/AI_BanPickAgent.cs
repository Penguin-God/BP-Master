
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


    void Select(Team team, SelectType selectType)
    {
        if (team != Team) return;
        if (selectType == SelectType.Ban) storage.SaveSelect(new SelectInfo(Team, selectType, banSelector.Ban(storage.SelectableIds)));
        else storage.SaveSelect(new SelectInfo(Team, selectType, pickSelector.Pick(storage.SelectableIds)));
    }

    public void Ban(Team team) => Select(team, SelectType.Ban);
    public void Pick(Team team) => Select(team, SelectType.Pick);
}
