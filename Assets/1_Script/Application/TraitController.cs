using System.Collections.Generic;

public class TraitController
{
    readonly IReadOnlyDictionary<Team, IReadOnlyList<Trait>> traits;

    public TraitController(IReadOnlyDictionary<Team, IReadOnlyList<Trait>> traits)
    {
        this.traits = traits;
    }

    public void ApplyTrait(ITraitAction action, IEnumerable<Champion> champions)
    {
        foreach (var champion in champions)
            champion.OnTrait(action);
    }

    public void ApplyTrait(Team team, int index, IEnumerable<Champion> champions)
    {
        foreach (var champion in champions)
            champion.OnTrait(traits[team][index].TraitAction);
    }
}
