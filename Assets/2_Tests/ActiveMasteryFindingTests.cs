using NUnit.Framework;

public class ActiveMasteryFindingTests
{
    [Test]
    public void 게이머와_같은_슬롯의_ID_숙련도_가져오기()
    {
        SlotStorage<ProGamer> gamers = new();
        gamers.AddSlot(Team.Blue, new ProGamer(new ChampionMastery[] { new ChampionMastery(1, 10) }));

        SlotStorage<int> ids = new();
        ids.AddSlot(Team.Blue, 1);

        ActiveMasteryFinder sut = new(gamers, ids);

        int result = sut.GetActiveLevel(TestHelper.CreateBlueSlot(0));

        Assert.AreEqual(10, result);
    }
}
