using NUnit.Framework;
using static TestHelper;

public class SelectChampsTests
{
    [Test]
    public void 가장_가치가_높은_챔피언_픽()
    {
        var statusSlots = CreateTwoSlotStatus();
        ChampionCatalog catalog = CreateCaltalog(CreateChampion(1, skillData : CreateValueSkillData(SkillType.AttackChanger, 100, rule: SelfAllRule)), CreateChampion(2), CreateChampion(3));
        ChampionStatValueCalculator statCalculator = new ChampionStatValueCalculator(speedValue: 10);
        //SkillApplyDeltaCalculator deltaCalculator = new SkillApplyDeltaCalculator(new SkillPreviewer(CreateSkillExceutorFactory(), statusSlots), statusSlots);
        //MasteryCollection masteryCollection = new MasteryCollection(new ChampionMastery[] { new ChampionMastery(1, 10) });

        // 계산식(스탯 밸류+ 마스터리 레벨 * 2 + 스킬 밸류)
        //ValuePick sut = new ValuePick(catalog, statCalculator, deltaCalculator, masteryCollection);

        //int result = sut.Pick(new HashSet<int>() { 1, 2, 3 });

        //Assert.AreEqual(1, result);
    }

    ChampionCatalog CreateCaltalog(params Champion[] champions) => new ChampionCatalog(champions);
}
