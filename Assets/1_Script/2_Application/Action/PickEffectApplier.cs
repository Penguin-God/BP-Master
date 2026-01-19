public class PickEffectApplier
{
    readonly MasteryApplier masteryApplier;

    public PickEffectApplier(MasteryCollection masteryCollection)
    {
        this.masteryApplier = new MasteryApplier(masteryCollection);
    }

    public void Apply(Team team, Champion champion)
    {
        masteryApplier.ApplyMastery(champion.Id, champion.Status);
    }
}