using NUnit.Framework;

public class PickStatusChangeTests
{
    SlotStorage<ChampionStatus> statusTable;

    [SetUp]
    public void Setup()
    {
        statusTable = new();
        statusTable.AddSlot(Team.Blue, TestHelper.CreateStatus(10, 20, 30));
        statusTable.AddSlot(Team.Red, TestHelper.CreateStatus(40, 50, 60));
    }

    [Test]
    public void 스탯_변경시_Status가_갱신되고_OnStatChanged_이벤트가_발행된다()
    {
        var sut = new PickStatusChanger(statusTable);

        StatChangeData received = default;
        sut.OnStatChanged += change => received = change;

        var slot = new SlotData(Team.Blue, 0);
        var newStat = new ChampionStatData(11, 21, 31);

        sut.ChangeStat(slot, newStat);

        // 저장소 실제 값 갱신 검증
        Assert.AreEqual(newStat, statusTable.GetSlot(slot).Stat);

        // 이벤트 페이로드 검증
        Assert.AreEqual(slot, received.Slot);
        Assert.AreEqual(new ChampionStatData(10, 20, 30), received.Before);
        Assert.AreEqual(newStat, received.After);
    }

    [Test]
    public void 동일한값으로_변경하면_아무이벤트도_발생하지_않는다()
    {
        var sut = new PickStatusChanger(statusTable);

        bool anyEvent = false;
        sut.OnStatChanged += _ => anyEvent = true;

        var slot = new SlotData(Team.Red, 0);
        var same = statusTable.GetSlot(slot).Stat;

        sut.ChangeStat(slot, same);

        Assert.IsFalse(anyEvent);
    }
}
