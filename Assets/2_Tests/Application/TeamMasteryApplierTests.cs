using NUnit.Framework;
using System.Collections.Generic;
using static TestHelper;

public class TeamMasteryApplier_GamersIds_Tests
{
    SlotStorage<ChampionStatus> statusTable;
    TeamMasteryApplier sut;

    [SetUp]
    public void Setup()
    {
        statusTable = new SlotStorage<ChampionStatus>();
        statusTable.AddSlot(Team.Blue, CreateStatus(10, 20, 30));
        statusTable.AddSlot(Team.Red, CreateStatus(40, 50, 60));

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

        sut.Apply(gamers, ids, statusTable);

        var result = statusTable.GetSlot(new SlotData(Team.Blue, 0)).Stat;
        Assert.AreEqual(new ChampionStatData(13, 23, 30), result);
    }

    [Test]
    public void 숙련도_보유한_챔은_스탯_증가()
    {
        var masteries = new[] { new ChampionMastery(1, 10) };
        Dictionary<int, ChampionStatus> statuses = new Dictionary<int, ChampionStatus>()
        {
            {1, CreateStatus() },
            {2, CreateStatus() }
        };
        var sut = new TeamMasteryApplier();

        sut.ApplyMastery(statuses, masteries);

        Assert.AreEqual(CreateStat(10, 10), statuses[1].Stat);
        Assert.AreEqual(CreateStat(0, 0), statuses[2].Stat);
    }
}
