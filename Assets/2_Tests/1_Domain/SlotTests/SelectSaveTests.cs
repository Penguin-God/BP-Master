using NUnit.Framework;
using System;
using static TestHelper;

public class SelectSaveTests
{
    [Test]
    public void 선택_불가능한_ID면_예외()
    {
        BanPickStorage sut = CreateStorage(1);

        sut.Ban(Team.Blue, 1);

        Assert.Throws<ArgumentException>(() => sut.Pick(Team.Blue, 1));
        Assert.Throws<ArgumentException>(() => sut.Ban(Team.Blue, 44));
    }

    [Test]
    public void Id_저장_후_저장한_슬롯_위치_반환()
    {
        var sut = CreateStorage(3, 4, 5);
        Assert.AreEqual(sut.Pick(Team.Blue, 3), BlueZeroSlot);
        Assert.AreEqual(sut.Pick(Team.Blue, 4), BlueOneSlot);
        Assert.AreEqual(sut.Pick(Team.Red, 5), RedZeroSlot);

        Assert.AreEqual(sut.PickIds.GetSlot(BlueZeroSlot), 3);
        Assert.AreEqual(sut.PickIds.GetSlot(BlueOneSlot), 4);
        Assert.AreEqual(sut.PickIds.GetSlot(RedZeroSlot), 5);
    }
}
