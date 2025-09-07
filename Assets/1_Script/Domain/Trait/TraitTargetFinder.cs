using System.Collections.Generic;
using System.Linq;

public class TraitTargetFinder
{
    readonly IReadOnlyDictionary<Team, IReadOnlyList<int>> teamMembers;
    
    public TraitTargetFinder(IReadOnlyDictionary<Team, IReadOnlyList<int>> teamMembers) => this.teamMembers = teamMembers;

    public IEnumerable<int> GetTargets(Team targetTeam, TargetRange range, int targetId)
    {
        if (teamMembers[targetTeam].Any(x => x == targetId) == false) return null;

        switch (range)
        {
            case TargetRange.Single: return new int[] { targetId };
            case TargetRange.All: return teamMembers[targetTeam];
            default: return null;
        }
    }
}
