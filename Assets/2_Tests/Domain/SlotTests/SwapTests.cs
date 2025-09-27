using NUnit.Framework;

public class SwapTests
{
    [Test]
    public void 저장소_스왑()
    {
        var storage = new GameBanPickStorage(new int[] { 11, 22 });

        storage.SaveSelect(new SelectInfo(Team.Blue, SelectType.Pick, 11));
        storage.SaveSelect(new SelectInfo(Team.Blue, SelectType.Pick, 22));

        storage.Swap(Team.Blue, 0, 1);
        Assert.AreEqual(22, storage.GetStorage(Team.Blue, SelectType.Pick)[0]);
        Assert.AreEqual(11, storage.GetStorage(Team.Blue, SelectType.Pick)[1]);
    }
}
