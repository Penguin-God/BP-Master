using NUnit.Framework;

public class TeamMasteryApplier_GamersIds_Tests
{
    SlotStorage<ChampionStatus> statusTable;
    SlotStatusChanger statusChanger;
    TeamMasteryApplier sut;

    [SetUp]
    public void Setup()
    {
        statusTable = new SlotStorage<ChampionStatus>();
        statusTable.AddSlot(Team.Blue, new ChampionStatus(new ChampionStatData(10, 20, 30)));
        statusTable.AddSlot(Team.Red, new ChampionStatus(new ChampionStatData(40, 50, 60)));

        statusChanger = new SlotStatusChanger(statusTable);
        sut = new TeamMasteryApplier(statusChanger);
    }

    [Test]
    public void 숙련도_적용시_스탯이_증가_및_이벤트()
    {
        var blueMasteries = new[] { new ChampionMastery(1, 3) };
        var redMasteries = new[] { new ChampionMastery(2, 0) };

        var gamers = new SlotStorage<ProGamer>();
        gamers.AddSlot(Team.Blue, new ProGamer(blueMasteries));
        gamers.AddSlot(Team.Red, new ProGamer(redMasteries));

        var ids = new SlotStorage<int>();
        ids.AddSlot(Team.Blue, 1); // Blue0 슬롯의 픽 ID
        ids.AddSlot(Team.Red, 2); // Red0  슬롯의 픽 ID

        StatChangeData received = default;
        statusChanger.OnStatChanged += change => received = change;

        // Action
        sut.Apply(gamers, ids);

        // Assert (Blue0만 확인)
        var changed = statusTable.GetSlot(new SlotData(Team.Blue, 0)).Stat;
        Assert.AreEqual(new ChampionStatData(13, 23, 30), changed);

        Assert.AreEqual(new SlotData(Team.Blue, 0), received.Slot);
        Assert.AreEqual(new ChampionStatData(10, 20, 30), received.Before);
        Assert.AreEqual(new ChampionStatData(13, 23, 30), received.After);
    }

    [Test]
    public void 숙련도0이면_이벤트_없음()
    {
        var gamers = new SlotStorage<ProGamer>();
        var redMasteries = new[] { new ChampionMastery(2, 0) };
        gamers.AddSlot(Team.Red, new ProGamer(redMasteries));

        var ids = new SlotStorage<int>();
        ids.AddSlot(Team.Red, 2);

        bool anyEvent = false;
        statusChanger.OnStatChanged += _ => anyEvent = true;

        // Action
        sut.Apply(gamers, ids);

        // Assert (Red0만 확인)
        Assert.IsFalse(anyEvent);
    }
}
