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

        var phaseAdvancer = new PhaseAdvancer(new PhaseData[] { new PhaseData(GamePhase.Pick, new Phase(new Team[] { Team.Blue, Team.Red })) });
        phaseAdvancer.Start(); // 현재 턴 활성화를 위해 Start 호출 필요
        var sut = new PredictivePickEvaluator(pickValueEvaluator, storage, catalog, previewer, phaseAdvancer, Team.Blue, originSlots);

        Assert.AreEqual(-4500, sut.Evaluate(champion));
        Assert.AreEqual(-30000, sut.Evaluate(champion2));
    }

    [Test]
    public void 자신이_픽을_2번하는_경우를_계산()
    {
        var champion = CreateChampion(1, att: 500);
        var champion2 = CreateChampion(2, att: 0, def: 3000);
        var champion3 = CreateChampion(3, att: 700);

        var storage = CreateStorage(1, 2, 3);
        var catalog = CreateCaltalog(champion, champion2, champion3);

        var originSlots = new SlotStorage<ChampionStatus>();
        var statCalculator = new ChampionStatValueCalculator(speedValue: 0);
        var previewer = new SkillPreviewer();
        var bonus = new BonusDeltaCalculator(new TeamBonusCalculator(CreateBonus(1000, 10000), CreateBonus(0, 0), CreateBonus(0, 0)));
        var pickValueEvaluator = new PickValueEvaluator(statCalculator, new ChampionValueCalculator(previewer, CreateMasteryApplier(new ChampionMastery(CHAMP_ID, 0))), bonus, Team.Blue, originSlots);

        var phaseAdvancer = new PhaseAdvancer(new PhaseData[] { new PhaseData(GamePhase.Pick, new Phase(new Team[] { Team.Blue, Team.Blue })) });
        phaseAdvancer.Start(); // 현재 턴 활성화를 위해 Start 호출 필요

        var sut = new PredictivePickEvaluator(pickValueEvaluator, storage, catalog, previewer, phaseAdvancer, Team.Blue, originSlots);

        
        Assert.AreEqual(11200, sut.Evaluate(champion));
        Assert.AreEqual(3700, sut.Evaluate(champion2));
        Assert.AreEqual(11200, sut.Evaluate(champion3));
    }
}