using System.Collections.Generic;
using System.Linq;

public class ActiveTargetPresenter
{
    readonly Team Team;
    readonly IReadOnlyDictionary<Team, IReadOnlyList<int>> teamMembers;

    public ActiveTargetPresenter(Team team, IReadOnlyDictionary<Team, IReadOnlyList<int>> teamMembers)
    {
        Team = team;
        this.teamMembers = teamMembers;
    }

    public IEnumerable<int> GetTargets(int id, Side side, TraitTargetType targetRange)
    {
        Team targetTeam = BanPickEnumCaster.GetTargetTeam(Team, side);
        // if (targetTeam == Team.All && targetRange == TraitTargetType.All) return teamMembers.Values.SelectMany(x => x).ToList(); // 당장은 무의미
        if (teamMembers[targetTeam].Contains(id) == false) return null;

        if (targetRange == TraitTargetType.Single) return new int[] { id };
        else if (targetRange == TraitTargetType.All) return teamMembers[targetTeam];
        return null;
    }
}
