using System.Collections.Generic;
public class ActiveExcuter
{
    readonly StatManager statManager;
    Queue<Trait> traits;
    public ActiveExcuter(StatManager statManager, IEnumerable<Trait> traits)
    {
        this.traits = new Queue<Trait>(traits);
        this.statManager = statManager;
    }

    public bool IsDone => traits.Count == 0;

    public void DoActive(int target)
    {
        Trait trait = traits.Dequeue();
        statManager.ChangeSelectData(trait.TargetSide, target, trait.TraitAction.Do);
    }
}
