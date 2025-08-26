using System.Collections.Generic;

public enum Side { Self, Opponent, All }
public class ActiveExcuter
{
    readonly StatManager statManager;
    readonly Team Team;
    Queue<Trait> traits;
    public ActiveExcuter(StatManager statManager, Team team, IEnumerable<Trait> traits)
    {
        this.traits = new Queue<Trait>(traits);
        this.statManager = statManager;
    }

    public ActiveExcuter(StatManager statManager, IEnumerable<Trait> traits)
    {
        this.traits = new Queue<Trait>(traits);
        this.statManager = statManager;
    }

    public bool IsDone => traits.Count == 0;

    public void DoActive(int target)
    {
        Trait trait = traits.Dequeue();
        // statManager.ChangeSelectData(trait.TargetSide, target, trait.TraitAction.Do);
    }
}
