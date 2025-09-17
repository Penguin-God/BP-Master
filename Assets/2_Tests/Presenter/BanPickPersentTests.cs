using NUnit.Framework;

public class BanPickPersentTests
{
    [Test]
    public void 현재_순서에_맞는_슬롯_반환()
    {
        var sut = new TeamSlotIndexr();

        Assert.AreEqual(0, sut.AllocateIndex(Team.Blue));
        Assert.AreEqual(0, sut.AllocateIndex(Team.Red));
        Assert.AreEqual(1, sut.AllocateIndex(Team.Red));
        Assert.AreEqual(1, sut.AllocateIndex(Team.Blue));
    }
}
