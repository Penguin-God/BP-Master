using System.Collections.Generic;
using System.Linq;

public class TraitTargetFinder
{
    readonly IReadOnlyDictionary<Team, IReadOnlyList<int>> teamIds;
    
    public TraitTargetFinder(IReadOnlyDictionary<Team, IReadOnlyList<int>> teamMembers) => this.teamIds = teamMembers;

    public IEnumerable<int> GetTargets(Team targetTeam, TargetRange range, int targetIndex)
    {
        switch (range)
        {
            case TargetRange.Single: return new int[] { teamIds[targetTeam][targetIndex] };
            case TargetRange.All: return teamIds[targetTeam];
            default: return null;
        }
    }
}
