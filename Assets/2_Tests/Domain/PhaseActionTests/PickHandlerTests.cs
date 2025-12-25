using NUnit.Framework;
using static TestHelper;

public class PickHandlerTests
{
    [Test]
    public void 픽하면_해당_팀에_챔피언이_슬롯에_추가됨()
    {
        const int CHAMP_ID = 1;
        var champion = new Champion(CHAMP_ID, null, CreateStatus(10, 10, 10, TraitType.Amplifier));
        var catalog = new ChampionCatalog(new Champion[] { champion });
        var slotFacade = new PickSlotFacade();

        var sut = new PickHandler(catalog, slotFacade);

        sut.Pick(Team.Blue, CHAMP_ID);

        Assert.IsNotNull(slotFacade.StatusSlots.GetSlot(BlueZeroSlot));
    }
}
