using System.Collections.Generic;
using System.Linq;

public class ActiveTargetPresenter
{
    readonly Team Team;
    readonly IReadOnlyDictionary<Team, IReadOnlyList<Champion>> teamMembers;
    IReadOnlyList<Champion> allChampion => teamMembers.Values.SelectMany(x => x).ToList();
    Champion selectChamp;
    Side TargetSide => selectChamp.Trait.TargetSide;
    TargetRange TargetRange => selectChamp.Trait.TargetRange;
    public ActiveTargetPresenter(Team team, IReadOnlyDictionary<Team, IReadOnlyList<Champion>> teamMembers)
    {
        Team = team;
        this.teamMembers = teamMembers;
    }

    public void Cancle() => selectChamp = null;

    public IEnumerable<int> GetTargetIds(int id) => GetTargets(id)?.Select(x => x.Id);
    public IEnumerable<Champion> GetTargets(int id)
    {
        if (selectChamp == null) return null;

        Team targetTeam = BanPickEnumCaster.GetTargetTeam(Team, TargetSide);
        // if (targetTeam == Team.All && targetRange == TraitTargetType.All) return teamMembers.Values.SelectMany(x => x).ToList(); // 당장은 무의미
        if (teamMembers[targetTeam].Any(x => x.Id == id) == false) return null;

        if (TargetRange == TargetRange.Single) return new Champion[] { FindChampion(id) };
        else if (TargetRange == TargetRange.All) return teamMembers[targetTeam];
        return null;
    }
    Champion FindChampion(int id) => allChampion.First(x => x.Id == id);

    public void SelectChamp(int id) => selectChamp = teamMembers[Team].First(x => x.Id == id);
}
