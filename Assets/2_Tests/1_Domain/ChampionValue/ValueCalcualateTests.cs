using NUnit.Framework;
using static TestHelper;

public class ValueCalcualateTests
{
    int CHAMP_ID = 1;
    PickValueEvaluator CreateSut(BonusCalculator bonusCalculator, int masteryLevel = 0, int att = 0)
    {
        var originSlots = CreateTwoSlotStatus(att);

        var statCalculator = new ChampionStatValueCalculator(speedValue: 0);
        var bonus = new BonusDeltaCalculator(new TeamBonusCalculator(bonusCalculator, bonusCalculator, bonusCalculator));
        return new PickValueEvaluator(statCalculator, new ChampionValueCalculator(CreateMasteryApplier(new ChampionMastery(CHAMP_ID, masteryLevel))), bonus, Team.Blue, originSlots);
    }

    [Test]
    public void 적군에게_이득을_주는_스킬은_점수가_차감된다()
    {
        var skillData = CreateAttackChangeSkill(100, rule: OpponentAllRule);
        var champion = CreateChampion(CHAMP_ID, skillData: skillData);
        var sut = CreateSut(CreateBonus(0, 0), masteryLevel: 0);

        int score = sut.Evaluate(champion);

        // 내 팀(Blue) 기준에서 적(Red)의 스탯이 올랐으므로 점수는 마이너스여야 함
        Assert.AreEqual(-200, score);
    }

    [Test]
    public void 적_보너스_떨구는만큼_가치_증가()
    {
        var skillData = CreateAttackChangeSkill(-100, rule: OpponentAllRule);
        var champion = CreateChampion(CHAMP_ID, skillData: skillData);
        var sut = CreateSut(CreateBonus(100, 30000), masteryLevel: 0, att: 100);

        int score = sut.Evaluate(champion);

        Assert.AreEqual(30200, score);
    }

    [Test]
    [TestCase(0, 320)] // 원래 받는 보너스는 점수 반영 X
    [TestCase(300, 420)] // stat 100 + mastery 20 + skill 200 + bonus 100
    public void 스탯_숙련도_스킬_보너스_적용한_챔피언_가치(int bounsNeed, int result)
    {
        var skillData = CreateAttackChangeSkill(100, rule: SelfAllRule);
        var champion = CreateChampion(CHAMP_ID, att: 100, skillData: skillData);
        var sut = CreateSut(CreateBonus(bounsNeed, 100), masteryLevel: 10);

        int score = sut.Evaluate(champion);

        Assert.AreEqual(result, score);
    }
}
