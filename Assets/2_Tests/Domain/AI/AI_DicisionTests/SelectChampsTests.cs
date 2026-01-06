using NUnit.Framework;
using System.Collections.Generic;
using static TestHelper;

public class SelectChampsTests
{
    [Test]
    public void 가장_가치가_높은_챔피언_픽()
    {
        const Team Team = Team.Blue;
        var statusSlots = CreateTwoSlotStatus();
        ChampionCatalog catalog = CreateCaltalog(CreateChamp(1, 100), CreateChamp(2, 0), CreateChamp(3, 0));
        ChampionStatValueCalculator statCalculator = new ChampionStatValueCalculator(speedValue: 10);
        SkillApplyDeltaCalculator deltaCalculator = new SkillApplyDeltaCalculator();
        MasteryCollection masteryCollection = new MasteryCollection(new ChampionMastery[] { new ChampionMastery(1, 10) });

        // 계산식(스탯 밸류+ 마스터리 레벨 * 2 + 스킬 밸류)
        ValuePick sut = new ValuePick(catalog, new ChampionValueCalculator(statCalculator, deltaCalculator, masteryCollection, Team));

        int result = sut.Pick(new HashSet<int>() { 1, 2, 3 });

        Assert.AreEqual(1, result);
    }
    Champion CreateChamp(int id, int value) => CreateChampion(id, skillData: CreateValueSkillData(SkillType.AttackChanger, value, rule: SelfAllRule));
    ChampionCatalog CreateCaltalog(params Champion[] champions) => new ChampionCatalog(champions);
}
