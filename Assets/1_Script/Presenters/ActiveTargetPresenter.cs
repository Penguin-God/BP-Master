using System.Collections.Generic;
using System.Linq;

public class ActiveTargetPresenter
{
    readonly Team Team;
    readonly IReadOnlyDictionary<Team, IReadOnlyList<int>> teamMembers;

    Side targetSide;
    TraitTargetType targetRange;

    public ActiveTargetPresenter(Team team, IReadOnlyDictionary<Team, IReadOnlyList<int>> teamMembers)
    {
        Team = team;
        this.teamMembers = teamMembers;
        targetSide = Side.Self;
        targetRange = TraitTargetType.None;
    }

    public void SelectTrait(Side side, TraitTargetType traitTargetType)
    {
        targetSide = side;
        targetRange = traitTargetType;
    }

    public IEnumerable<int> GetTargets(int id)
    {
        Team targetTeam = BanPickEnumCaster.GetTargetTeam(Team, targetSide);
        if (targetTeam == Team.All && targetRange == TraitTargetType.All) return teamMembers.Values.SelectMany(x => x).ToList();
        if (teamMembers[targetTeam].Contains(id) == false) return null;

        if (targetRange == TraitTargetType.Single) return new int[] { id };
        else if (targetRange == TraitTargetType.All) return teamMembers[targetTeam];
        return null;
    }
}
