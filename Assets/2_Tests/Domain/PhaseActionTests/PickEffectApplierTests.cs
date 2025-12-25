using NUnit.Framework;
using static TestHelper;

public class PickEffectApplierTests
{
    const int CHAMP_ID = 1;
    const int MASTERY_LV = 10;
    
    [Test]
    public void 챔피언_특성과_숙련도_적용()
    {
        var champion = new Champion(CHAMP_ID, null, CreateStatus(10, 10, 10, TraitType.Amplifier));
        var masteries = new MasteryCollection(new[] { new ChampionMastery(CHAMP_ID, MASTERY_LV) });
        SlotStorage<ChampionStatus> statusSlots = new();
        statusSlots.AddSlot(Team.Blue, champion.Status);
        var traitFactory = new TraitFactory(new TraitConfig(0, 0, AmpilyRate: 0.1f, 0), statusSlots);

        var sut = new PickEffectApplier(traitFactory, masteries);

        sut.Apply(Team.Blue, champion);

        Assert.AreEqual(20, champion.Status.Stat.Attack, "숙련도에 의해 공격력이 증가해야 한다.");
        Assert.AreEqual(20, champion.Status.Stat.Defense, "숙련도에 의해 방어력이 증가해야 한다.");
        Assert.AreEqual(1.1f, champion.Status.UpRate, "증폭 특성이 적용되어야 한다.");
    }
}