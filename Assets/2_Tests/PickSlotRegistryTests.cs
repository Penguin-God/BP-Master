using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

public class PickSlotRegistryTests
{
    private SlotStorage<ProGamer> gamers;
    private PickSlotRegistry sut;

    [SetUp]
    public void Setup()
    {
        gamers = new SlotStorage<ProGamer>();

        gamers.AddSlot(Team.Blue, new ProGamer(new[] { new ChampionMastery(7, 10) }));
        gamers.AddSlot(Team.Blue, new ProGamer(new[] { new ChampionMastery(2, 3) }));

        gamers.AddSlot(Team.Red, new ProGamer(new[] { new ChampionMastery(9, 5) }));

        sut = new PickSlotRegistry(gamers);
    }

    void Pick(Team team, int id) => sut.Pick(team, id);

    [Test]
    public void 같은_팀_픽은_인덱스가_공유_증가()
    {
        Pick(Team.Blue, 7); // Blue 0
        Pick(Team.Blue, 2); // Blue 1

        var list = sut.PickSlotDatas.ToList();
        Assert.AreEqual(2, list.Count);

        Assert.AreEqual(Team.Blue, list[0].Slot.Team);
        Assert.AreEqual(0, list[0].Slot.Index);
        Assert.AreEqual(7, list[0].ChampId);

        Assert.AreEqual(Team.Blue, list[1].Slot.Team);
        Assert.AreEqual(1, list[1].Slot.Index);
        Assert.AreEqual(2, list[1].ChampId);
    }

    [Test]
    public void 서로_다른_팀은_각자_인덱스를_독립적으로_가진다()
    {
        Pick(Team.Blue, 7); // Blue 0
        Pick(Team.Red, 9);  // Red 0
        Pick(Team.Blue, 2); // Blue 1

        var list = sut.PickSlotDatas.ToList();
        Assert.AreEqual(3, list.Count);

        // Blue 0
        Assert.AreEqual(Team.Blue, list[0].Slot.Team);
        Assert.AreEqual(0, list[0].Slot.Index);

        // Red 0
        Assert.AreEqual(Team.Red, list[1].Slot.Team);
        Assert.AreEqual(0, list[1].Slot.Index);

        // Blue 1
        Assert.AreEqual(Team.Blue, list[2].Slot.Team);
        Assert.AreEqual(1, list[2].Slot.Index);
    }

    [Test]
    public void 슬롯의_활성_숙련도_조회가_올바르다()
    {
        Pick(Team.Blue, 7); // Blue 0: mastery 10
        Pick(Team.Blue, 2); // Blue 1: mastery 3
        Pick(Team.Red, 9);  // Red 0 : mastery 5

        var list = sut.PickSlotDatas.ToList();

        var blue0 = list[0];
        var blue1 = list[1];
        var red0 = list[2];

        Assert.AreEqual(10, blue0.GetActiveMastery());
        Assert.AreEqual(3, blue1.GetActiveMastery());
        Assert.AreEqual(5, red0.GetActiveMastery());
    }
}
