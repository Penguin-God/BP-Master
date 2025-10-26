using System.Collections.Generic;

public class TraitExecutor
{
    readonly TraitFactory traitFactory;
    public TraitExecutor(TraitFactory traitFactory) => this.traitFactory = traitFactory;

    public void ExecuteAllTriat(SlotStorage<ChampionStatus> statusSlots)
    {
        ExecteTeamTrait(Team.Blue, statusSlots.GetTeam(Team.Blue));
        ExecteTeamTrait(Team.Red, statusSlots.GetTeam(Team.Red));
    }

    void ExecteTeamTrait(Team team, IEnumerable<ChampionStatus> statuses)
    {
        foreach (var status in statuses)
            traitFactory.Create(team, status.TraitType).Do();
    }
}
