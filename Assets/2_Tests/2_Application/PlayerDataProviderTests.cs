using NUnit.Framework;

public class PlayerDataProviderTests
{
    [Test]
    public void 플레이어_ID와_같으면_로컬로더_아니면_ai로더를_사용한다()
    {
        var localData = new PlayerData(1, "Player", null);
        var localLoader = CreateDummyLoader(localData);

        var aiData = new PlayerData(2, "AI", null);
        var aiLoader = CreateDummyLoader(aiData);

        var provider = new PlayerDataProvider(1, localLoader, aiLoader);

        var player = provider.GetPlayer(1);
        var ai = provider.GetPlayer(2);

        Assert.AreEqual(localData, player);
        Assert.AreEqual(aiData, ai);
    }

    IPlayerDataLoader CreateDummyLoader(PlayerData dataToReturn) => new DummyLoader(dataToReturn);

    class DummyLoader : IPlayerDataLoader
    {
        readonly PlayerData data;
        public DummyLoader(PlayerData data) => this.data = data;
        public PlayerData LoadPlayer(int id) => data;
    }
}