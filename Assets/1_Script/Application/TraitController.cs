using System.Collections.Generic;

public class TraitController
{
    public void ApplyTrait(ITraitAction action, IEnumerable<Champion> champions)
    {
        foreach (var champion in champions)
            champion.OnTrait(action);
    }
}
