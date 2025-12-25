public class PickEffectApplier
{
    readonly TraitFactory traitFactory;
    readonly MasteryCollection masteryCollection;

    public PickEffectApplier(TraitFactory traitFactory, MasteryCollection masteryCollection)
    {
        this.traitFactory = traitFactory;
        this.masteryCollection = masteryCollection;
    }

    public void Apply(Team team, Champion champion)
    {
        ApplyTrait(team, champion.Status.TraitType);

        if (masteryCollection.HasMastery(champion.Id))
        {
            int level = masteryCollection.GetMasteryLevel(champion.Id);
            new MasteryApplier().ApplyStatChange(champion.Status, level);
        }
    }

    void ApplyTrait(Team team, TraitType traitType) => traitFactory.Create(team, traitType).Do();
}