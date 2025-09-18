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
    readonly TraitTargetSelector targetFinder;
    readonly SlotStorage<bool> traitUseFlags;

    public TraitController(IReadOnlyDictionary<Team, IReadOnlyList<Champion>> traitsByTeam)
    {
        this.championsByTeam = traitsByTeam;
        targetFinder = new TraitTargetSelector(traitsByTeam[Team.Blue].Count);
        traitUseFlags = new SlotStorage<bool>(traitsByTeam[Team.Blue].Count, false);
    }

    public bool UseTrait(SlotData traitSlot, SlotData targetSlot)
    {
        if (IsTraitUsed(traitSlot)) return false;

        Champion champion = championsByTeam[traitSlot.Team][traitSlot.Index];
        var targets = targetFinder.GetTargetSlots(traitSlot.Team, champion.TraitTargetRule, targetSlot);
        ExecuteTrait(champion.TraitExecutor, targets.Select(x => championsByTeam[x.Team][x.Index]));
        traitUseFlags.ChangeSlot(traitSlot, true);
        return true;
    }

    public bool IsTraitUsed(SlotData slot) => traitUseFlags.GetSlot(slot);

    void ExecuteTrait(TraitExecutor executor, IEnumerable<Champion> champions)
    {
        foreach (var champion in champions)
            executor.ExecteTrait(champion);
    }

    public int GetTeamSize(Team team) => championsByTeam[team].Count;

    public TraitTargetRule GetTargetRule(Team team, int index) => championsByTeam[team][index].TraitTargetRule;
}
