using System;
using System.Collections.Generic;
using System.Linq;

public class TraitPresenter
{
    readonly Team Team;
    readonly IReadOnlyDictionary<Team, IReadOnlyList<Champion>> teamMembers;
    IReadOnlyList<Champion> allChampion => teamMembers.Values.SelectMany(x => x).ToList();
    Champion selectChamp;

    public void SelectChamp(int id) => selectChamp = teamMembers[Team].First(x => x.Id == id);

    public bool UseTrait(int targetIndex)
    {
        return false;
    }
}
