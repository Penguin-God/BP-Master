using NUnit.Framework;
using static TestHelper;

public class TeamMasteryApplier_GamersIds_Tests
{
    [Test]
    public void 숙련도_보유한_챔은_스탯_증가()
    {
        var masteries = new[] { new ChampionMastery(1, 10) };
        var sut = new TeamMasteryApplier();
        var statuses = new ChampionStatus[] { CreateStatus(), CreateStatus() };
        sut.ApplyMastery(new int[] { 1, 2 }, statuses, masteries);

        Assert.AreEqual(CreateStat(10, 10), statuses[0].Stat);
        Assert.AreEqual(CreateStat(0, 0), statuses[1].Stat);
    }
}
