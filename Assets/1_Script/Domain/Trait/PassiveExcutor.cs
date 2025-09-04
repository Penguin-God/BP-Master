using System.Collections.Generic;
using System.Linq;

public class PassiveExcutor
{
    readonly StatManager _statManager;
    readonly Trait[] _blueTraits;
    readonly Trait[] _redTraits;

    public PassiveExcutor(StatManager statManager, IEnumerable<Trait> blueTraits, IEnumerable<Trait> redTraits)
    {
        _statManager = statManager;
        _blueTraits = blueTraits.ToArray();
        _redTraits = redTraits.ToArray();
    }

    public void Do()
    {
        ApplyPassives(Team.Blue, _blueTraits);
        ApplyPassives(Team.Red, _redTraits);
    }

    void ApplyPassives(Team ownerTeam, IEnumerable<Trait> traits)
    {
        foreach (var trait in traits)
        {
            var targetTeam = BanPickEnumCaster.GetTargetTeam(ownerTeam, trait.TargetSide);
            ApplyPassive(targetTeam, trait.TraitAction);
        }
    }

    void ApplyPassive(Team team, ITraitAction action)
    {
        if (team == Team.All)
        {
            _statManager.ChangeAll(Team.Blue, action.Do);
            _statManager.ChangeAll(Team.Red, action.Do);
        }
        else _statManager.ChangeAll(team, action.Do);
    }
}
