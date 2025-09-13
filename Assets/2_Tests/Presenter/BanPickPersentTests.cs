using NUnit.Framework;

public class BanPickPersentTests
{
    [Test]
    public void 현재_순서에_맞는_슬롯_반환()
    {
        var sut = new BanPickPersenter();

        Assert.AreEqual(TestHelper.CreateBlueSlot(0), sut.GetNextSlot(Team.Blue));
        Assert.AreEqual(TestHelper.CreateRedSlot(0), sut.GetNextSlot(Team.Red));
        Assert.AreEqual(TestHelper.CreateRedSlot(1), sut.GetNextSlot(Team.Red));
        Assert.AreEqual(TestHelper.CreateBlueSlot(1), sut.GetNextSlot(Team.Blue));
    }
}
