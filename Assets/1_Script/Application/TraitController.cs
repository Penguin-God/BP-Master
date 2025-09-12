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

    public bool UseTrait(ChampionSlot traitSlot, ChampionSlot targetSlot)
    {
        if (traitUseFlags[traitSlot.Team][traitSlot.Index] == true) return false;

        Trait trait = championsByTeam[traitSlot.Team][traitSlot.Index].Trait;
        var targets = targetFinder.GetTargetSlots(traitSlot.Team, trait.TargetSide, trait.TargetRange, targetSlot);
        ExecuteTrait(trait.TraitAction, targets.Select(x => championsByTeam[x.Team][x.Index]));
        traitUseFlags[traitSlot.Team][traitSlot.Index] = true;
        return true;
    }

    public bool IsTraitUsed(ChampionSlot slot) => traitUseFlags[slot.Team][slot.Index];

    void ExecuteTrait(ITraitAction action, IEnumerable<Champion> champions)
    {
        foreach (var champion in champions)
            champion.OnTrait(action);
    }

    public int GetTeamSize(Team team) => championsByTeam[team].Count;

    public Trait GetTrait(Team team, int index) => championsByTeam[team][index].Trait;
}
