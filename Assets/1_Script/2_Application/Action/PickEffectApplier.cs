public class PickEffectApplier
{
    readonly TraitFactory traitFactory;
    readonly MasteryApplier masteryApplier;

    public PickEffectApplier(TraitFactory traitFactory, MasteryCollection masteryCollection)
    {
        this.traitFactory = traitFactory;
        this.masteryApplier = new MasteryApplier(masteryCollection);
    }

    public void Apply(Team team, Champion champion)
    {
        ApplyTrait(team, champion.Status.TraitType);
        masteryApplier.ApplyMastery(champion.Id, champion.Status);
    }

    void ApplyTrait(Team team, TraitType traitType) => traitFactory.Create(team, traitType).Do();
}