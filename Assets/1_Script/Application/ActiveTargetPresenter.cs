using System.Collections.Generic;
using System.Linq;

public class ActiveTargetPresenter
{
    readonly Team Team;
    readonly IReadOnlyDictionary<Team, IReadOnlyList<Champion>> teamMembers;
    Champion selectChamp;
    public ActiveTargetPresenter(Team team, IReadOnlyDictionary<Team, IReadOnlyList<Champion>> teamMembers)
    {
        Team = team;
        this.teamMembers = teamMembers;
    }

    public void Cancle() => selectChamp = null;

    public IEnumerable<int> GetTargetIds(int id)
    {
        if (selectChamp == null) return null;

        return new int[] { 11 };
    }

    //public object GetTargetIds(int id)
    //{
    //    Team targetTeam = BanPickEnumCaster.GetTargetTeam(Team, side);
    //    // if (targetTeam == Team.All && targetRange == TraitTargetType.All) return teamMembers.Values.SelectMany(x => x).ToList(); // 당장은 무의미
    //    if (teamMembers[targetTeam].Contains(id) == false) return null;

    //    if (targetRange == TargetRange.Single) return new int[] { id };
    //    else if (targetRange == TargetRange.All) return teamMembers[targetTeam];
    //    return null;
    //}

    public void SelectChamp(int id) => selectChamp = teamMembers[Team].First(x => x.Id == id);
}
