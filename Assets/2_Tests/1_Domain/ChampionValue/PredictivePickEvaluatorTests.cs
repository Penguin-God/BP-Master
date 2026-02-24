using NUnit.Framework;
using static TestHelper;

public class PredictivePickEvaluatorTests
{
    const int CHAMP_ID = 1;

    [Test]
    public void 픽을_2번하는_경우를_계산해_가장_높은_점수_선택()
    {
        var champion = CreateChampion(CHAMP_ID, 500);
        var originSlots = new SlotStorage<ChampionStatus>();
        originSlots.AddSlot(Team.Blue, CreateStatus(500));
        var statCalculator = new ChampionStatValueCalculator(speedValue: 0);
        var previewer = new SkillPreviewer();
        var bonus = new BonusDeltaCalculator(new TeamBonusCalculator(CreateBonus(100, 30000), CreateBonus(0, 0), CreateBonus(0, 0)));
        var pickValueEvaluator = new PickValueEvaluator(statCalculator, new ChampionValueCalculator(previewer, CreateMasteryApplier(new ChampionMastery(CHAMP_ID, 0))), bonus, Team.Blue, originSlots);

        var sut = new PredictivePickEvaluator(pickValueEvaluator);

        int score = sut.Evaluate(champion);

        Assert.AreEqual(30500, score);
    }
}