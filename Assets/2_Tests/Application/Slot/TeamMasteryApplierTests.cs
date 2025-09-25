using NUnit.Framework;

public class TeamMasteryApplierTests
{
    SlotStorage<ChampionStatus> statusTable;
    SlotStatusChanger statusChanger;

    // 헬퍼: PickSlotData 생성 (게이머/슬롯/챔프ID 세팅)
    private static PickSlotData CreatePickSlotData(Team team, int index, int championId, int masteryLevel)
    {
        var gamer = new ProGamer(new[] { new ChampionMastery(championId, masteryLevel) });
        var slot = new SlotData(team, index);
        var data = new PickSlotData(slot, gamer);
        data.Pick(championId);
        return data;
    }

    [SetUp]
    public void Setup()
    {
        statusTable = new SlotStorage<ChampionStatus>();
        statusTable.AddSlot(Team.Blue, new ChampionStatus(new ChampionStatData(10, 20, 30)));
        statusTable.AddSlot(Team.Red, new ChampionStatus(new ChampionStatData(40, 50, 60)));

        statusChanger = new SlotStatusChanger(statusTable);
    }

    [Test]
    public void 숙련도_적용시_스탯이_증가하고_이벤트가_발행된다()
    {
        var dataBlue0 = CreatePickSlotData(Team.Blue, 0, championId: 7, masteryLevel: 5);

        StatChangeData received = default;
        statusChanger.OnStatChanged += change => received = change;

        var sut = new TeamMasteryApplier(statusChanger);

        // Action
        sut.Apply(new[] { dataBlue0 });

        // Assert
        var result = statusTable.GetSlot(new SlotData(Team.Blue, 0)).Stat;
        Assert.AreEqual(new ChampionStatData(15, 25, 30), result);
        Assert.AreEqual(new SlotData(Team.Blue, 0), received.Slot);
        Assert.AreEqual(new ChampionStatData(10, 20, 30), received.Before);
        Assert.AreEqual(new ChampionStatData(15, 25, 30), received.After);
    }

    [Test]
    public void 숙련도0이면_스탯변경도_이벤트도_없다()
    {
        var dataRed0 = CreatePickSlotData(Team.Red, 0, championId: 2, masteryLevel: 0);

        bool anyEvent = false;
        statusChanger.OnStatChanged += _ => anyEvent = true;
        var sut = new TeamMasteryApplier(statusChanger);

        sut.Apply(new[] { dataRed0 });

        Assert.IsFalse(anyEvent);
    }
}
