using NUnit.Framework;
using static TestHelper;

public class PickHandlerTests
{
    [Test]
    public void 픽하면_해당_팀에_챔피언이_슬롯에_추가되고_특성_및_숙련도_적용()
    {
        const int CHAMP_ID = 1;
        var champion = new Champion(CHAMP_ID, null, CreateStatus(10, 10, 10, TraitType.Amplifier));
        var catalog = new ChampionCatalog(new Champion[] { champion });
        var masteries = new MasteryCollection(new ChampionMastery[] { new ChampionMastery(CHAMP_ID, 10) });
        var slotFacade = new PickSlotFacade();
        var traitFactory = new TraitFactory(new TraitConfig(0, 0, ampliRate: 0.1f, 0), slotFacade.StatusSlots);

        var sut = new PickHandler(catalog, slotFacade, traitFactory, masteries);

        sut.Pick(Team.Blue, CHAMP_ID);

        Assert.AreEqual(20, slotFacade.StatusSlots.GetSlot(BlueZeroSlot).Stat.Attack);
        Assert.AreEqual(1.1f, slotFacade.StatusSlots.GetSlot(BlueZeroSlot).UpRate);
    }
}
