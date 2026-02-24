using NUnit.Framework;
using static TestHelper;

public class PredictivePickEvaluatorTests
{
    const int CHAMP_ID = 1;

    [Test]
    public void 픽을_2번하는_경우를_계산()
    {
        var champion = CreateChampion(1, att: 500);
        var champion2 = CreateChampion(2, def: 500);

        var storage = CreateStorage(1, 2, 3);
        var catalog = CreateCaltalog(champion, champion2, CreateChampion(3, att: 0, def: 5000));

        var originSlots = new SlotStorage<ChampionStatus>();
        originSlots.AddSlot(Team.Blue, CreateStatus(500));
        var statCalculator = new ChampionStatValueCalculator(speedValue: 0);
        var previewer = new SkillPreviewer();
        var bonus = new BonusDeltaCalculator(new TeamBonusCalculator(CreateBonus(100, 30000), CreateBonus(0, 0), CreateBonus(0, 0)));
        var pickValueEvaluator = new PickValueEvaluator(statCalculator, new ChampionValueCalculator(previewer, CreateMasteryApplier(new ChampionMastery(CHAMP_ID, 0))), bonus, Team.Blue, originSlots);

        var sut = new PredictivePickEvaluator(pickValueEvaluator, storage, catalog, previewer, Team.Blue, originSlots);

        Assert.AreEqual(-4500, sut.Evaluate(champion));
        Assert.AreEqual(-30000, sut.Evaluate(champion2));
    }

    [Test]
    public void 픽을_2번하는_경우를_계산2()
    {
        var champion = CreateChampion(2, att: 0, def: 3000);

        var storage = CreateStorage(1, 2, 3);
        var catalog = CreateCaltalog(CreateChampion(1, att: 500), champion, CreateChampion(3, att: 0, def: 5000), CreateChampion(4, att: 500));

        var originSlots = new SlotStorage<ChampionStatus>();
        originSlots.AddSlot(Team.Blue, CreateStatus(500));
        var statCalculator = new ChampionStatValueCalculator(speedValue: 0);
        var previewer = new SkillPreviewer();
        var bonus = new BonusDeltaCalculator(new TeamBonusCalculator(CreateBonus(100, 30000), CreateBonus(0, 0), CreateBonus(0, 0)));
        var pickValueEvaluator = new PickValueEvaluator(statCalculator, new ChampionValueCalculator(previewer, CreateMasteryApplier(new ChampionMastery(CHAMP_ID, 0))), bonus, Team.Blue, originSlots);

        var sut = new PredictivePickEvaluator(pickValueEvaluator, storage, catalog, previewer, Team.Blue, originSlots);

        int score = sut.Evaluate(champion);

        Assert.AreEqual(-27500, score);
    }
}