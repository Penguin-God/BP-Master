using System.Collections.Generic;
using System.Linq;

public class ActiveExcuter
{
    readonly StatManager statManager;
    Trait[] traits;
    public ActiveExcuter(StatManager statManager, IEnumerable<Trait> traits)
    {
        this.traits = traits.ToArray();
        this.statManager = statManager;
    }

    public void DoActive(int target)
    {
        statManager.ChangeSelectData(Side.Opponent, target, traits[0].Do);
    }
}
