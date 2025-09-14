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

        Champion champion = championsByTeam[traitSlot.Team][traitSlot.Index];
        var targets = targetFinder.GetTargetSlots(traitSlot.Team, champion.TraitTargetRule.TargetSide, champion.TraitTargetRule.TargetRange, targetSlot);
        ExecuteTrait(champion.TraitExecutor.Action, targets.Select(x => championsByTeam[x.Team][x.Index]));
        traitUseFlags[traitSlot.Team][traitSlot.Index] = true;
        return true;
    }

    public bool IsTraitUsed(ChampionSlot slot) => traitUseFlags[slot.Team][slot.Index];

    void ExecuteTrait(ITraitAction action, IEnumerable<Champion> champions)
    {
        foreach (var champion in champions)
            action.Do(champion);
    }

    public int GetTeamSize(Team team) => championsByTeam[team].Count;

    public TraitTargetRule GetTargetRule(Team team, int index) => championsByTeam[team][index].TraitTargetRule;
}
