using NUnit.Framework;

public class SwapTests
{
    [Test]
    [TestCase(0, 3)]
    [TestCase(2, 3)]
    [TestCase(1, 0)]
    public void 스왑_검증(int index1, int index2)
    {
        var sut = new TeamBanPickStorage();
        sut.SaveSelect(SelectType.Pick, 1);
        sut.SaveSelect(SelectType.Pick, 2);
        sut.SaveSelect(SelectType.Pick, 3);
        sut.SaveSelect(SelectType.Pick, 4);

        var list = sut.GetStorage(SelectType.Pick);
        int expect1 = list[index1];
        int expect2 = list[index2];

        // action
        sut.Swap(index1, index2);
        
        var result = sut.GetStorage(SelectType.Pick);
        Assert.AreEqual(expect1, result[index2]);  // ← 스왑 후 index2 위치에는 원래 index1 값
        Assert.AreEqual(expect2, result[index1]);  // ← 스왑 후 index1 위치에는 원래 index2 값
    }

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
