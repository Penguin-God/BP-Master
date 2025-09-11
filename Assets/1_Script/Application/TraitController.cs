using System.Collections.Generic;
using System.Linq;

public enum TraitType
{
    None,
    AttackChanger,
    DefenseChanger,
    SpeedChanger,
}

public class TraitController
{
    readonly IReadOnlyDictionary<Team, IReadOnlyList<Champion>> championsByTeam;
    readonly IReadOnlyDictionary<Team, bool[]> traitUseFlags;
    readonly TraitTargetSelector targetFinder;
    

    public TraitController(IReadOnlyDictionary<Team, IReadOnlyList<Champion>> traitsByTeam)
    {
        this.championsByTeam = traitsByTeam;
        targetFinder = new TraitTargetSelector(traitsByTeam[Team.Blue].Count);
        traitUseFlags = traitsByTeam.ToDictionary(x => x.Key, x => new bool[x.Value.Count]);
    }

    public bool UseTrait(Team team, int traitIndex, int targetIndex)
    {
        if (traitUseFlags[team][targetIndex] == true) return false;
        
        Trait trait = championsByTeam[team][targetIndex].Trait;
        var targetIds = targetFinder.GetTargetIds(trait.TargetRange, targetIndex);
        ExecuteTrait(trait.TraitAction, targetIds.Select(x => championsByTeam[BanPickEnumCaster.GetTargetTeam(team, trait.TargetSide)][x]));
        traitUseFlags[team][targetIndex] = true;
        return true;
    }

    void ExecuteTrait(ITraitAction action, IEnumerable<Champion> champions)
    {
        foreach (var champion in champions)
            champion.OnTrait(action);
    }
}
