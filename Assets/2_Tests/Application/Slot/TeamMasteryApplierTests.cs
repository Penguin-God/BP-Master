using NUnit.Framework;

public class TeamMasteryApplier_GamersIds_Tests
{
    SlotStorage<ChampionStatus> statusTable;
    TeamMasteryApplier sut;

    [SetUp]
    public void Setup()
    {
        statusTable = new SlotStorage<ChampionStatus>();
        statusTable.AddSlot(Team.Blue, new ChampionStatus(new ChampionStatData(10, 20, 30)));
        statusTable.AddSlot(Team.Red, new ChampionStatus(new ChampionStatData(40, 50, 60)));

        sut = new TeamMasteryApplier();
    }

    [Test]
    public void 숙련도_적용시_스탯_증가()
    {
        var gamers = new SlotStorage<ProGamer>();
        var blueMasteries = new[] { new ChampionMastery(1, 3) };
        gamers.AddSlot(Team.Blue, new ProGamer(blueMasteries));

        var ids = new SlotStorage<int>();
        ids.AddSlot(Team.Blue, 1);

        // Action
        sut.Apply(gamers, ids, statusTable);

        // Assert
        var result = statusTable.GetSlot(new SlotData(Team.Blue, 0)).Stat;
        Assert.AreEqual(new ChampionStatData(13, 23, 30), result);
    }
}
