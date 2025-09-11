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
    Champion selectChamp;
    public bool IsSelected => selectChamp != null;
    
    public TraitController(IReadOnlyDictionary<Team, IReadOnlyList<Champion>> traitsByTeam)
    {
        this.championsByTeam = traitsByTeam;
        targetFinder = new TraitTargetSelector(traitsByTeam[Team.Blue].Count);
    }

    public void SelectTrait(Team team, int index)
    {
        selectChamp = championsByTeam[team][index];
    }

    public bool UseTrait(Team team, int targetIndex)
    {
        if (IsSelected == false) return false;

        var targetIds = targetFinder.GetTargetIds(selectChamp.Trait.TargetRange, targetIndex);
        ExecuteTrait(selectChamp.Trait.TraitAction, targetIds.Select(x => championsByTeam[BanPickEnumCaster.GetTargetTeam(team, selectChamp.Trait.TargetSide)][x]));
        selectChamp = null;
        return true;
    }

    void ExecuteTrait(ITraitAction action, IEnumerable<Champion> champions)
    {
        foreach (var champion in champions)
            champion.OnTrait(action);
    }
}
