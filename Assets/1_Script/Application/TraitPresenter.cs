using System.Collections.Generic;
using System.Linq;

public class TraitPresenter
{
    readonly IReadOnlyDictionary<Team, IReadOnlyList<Champion>> championsByTeam;
    readonly TraitTargetSeletor targetFinder;
    Champion selectChamp;
    public bool IsSelected => selectChamp != null;
    Team selectTeam;

    public TraitPresenter(IReadOnlyDictionary<Team, IReadOnlyList<Champion>> traitsByTeam)
    {
        this.championsByTeam = traitsByTeam;
        targetFinder = new TraitTargetSeletor(traitsByTeam[Team.Blue].Count);
    }

    public void SelectTrait(Team team, int index)
    {
        selectTeam = team;
        selectChamp = championsByTeam[team][index];
    }

    public bool UseTrait(int targetIndex)
    {
        if (IsSelected == false) return false;

        var targetIds = targetFinder.GetTargetIds(selectChamp.Trait.TargetRange, targetIndex);
        new TraitController().ApplyTrait(selectChamp.Trait.TraitAction, targetIds.Select(x => championsByTeam[BanPickEnumCaster.GetTargetTeam(selectTeam, selectChamp.Trait.TargetSide)][x]));
        selectChamp = null;
        return true;
    }
}
