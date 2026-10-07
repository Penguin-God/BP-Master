using System.Collections.Generic;

public class MasteryRegistry
{
    readonly Dictionary<Team, MasteryStatCollection> teamMasteryMap;

    public MasteryRegistry() => teamMasteryMap = new Dictionary<Team, MasteryStatCollection>();

    public MasteryRegistry(MasteryStatCollection blueMastery, MasteryStatCollection redMastery) : this()
    {
        InitTeamMastery(Team.Blue, blueMastery);
        InitTeamMastery(Team.Red, redMastery);
    }

    public void InitTeamMastery(Team team, MasteryStatCollection collection) => teamMasteryMap[team] = collection;

    public MasteryStatCollection GetTeamMasteryCollection(Team team)
    {
        if(teamMasteryMap.ContainsKey(team)) return teamMasteryMap[team];
        else return null;
    }
}