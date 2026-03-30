using NUnit.Framework;

public class PlayerDataProviderTests
{
    [Test]
    public void 플레이어_ID와_같으면_로컬로더_아니면_ai로더를_사용한다()
    {
        var localData = new PlayerData(1, "Player", null);
        var aiData = new PlayerData(2, "AI", null);

        var sut = CreateSut(1, localData, aiData);

        var player = sut.GetPlayer(1);
        var ai = sut.GetPlayer(2);

        Assert.AreEqual(localData, player);
        Assert.AreEqual(aiData, ai);
    }

    [Test]
    public void 블루_레드_ID를_전달하면_팀별_플레이어_데이터_딕셔너리를_반환한다()
    {
        var localData = CreatePlayerData(1, "Player");
        var aiData = CreatePlayerData(2, "AI");
        var sut = CreateSut(1, localData, aiData);

        var dict = sut.GetTeamPlayersDict(1, 2);

        Assert.AreEqual(localData, dict[Team.Blue]);
        Assert.AreEqual(aiData, dict[Team.Red]);
    }

    PlayerData CreatePlayerData(int id, string name) => new PlayerData(id, name, null);

    PlayerDataProvider CreateSut(int mainId, PlayerData localData, PlayerData aiData) => new PlayerDataProvider(mainId, new DummyLoader(localData), new DummyLoader(aiData));

    class DummyLoader : IPlayerDataLoader
    {
        readonly PlayerData data;
        public DummyLoader(PlayerData data) => this.data = data;
        public PlayerData LoadPlayer(int id) => data;
    }
}