using NUnit.Framework;

public class SelectPresentTests
{
    [Test]
    public void 챔피언_선택_후_확정()
    {
        GameBanPickStorage storage = new(new int[] { 1, 2, 3 });
        ChampionSelectPresenter sut = new(storage);

        sut.SelectChamp(1);
        sut.NailDownChampion(new GameFlowData(GamePhase.Pick, Team.Blue));

        Assert.AreEqual(1, storage.GetStorage(Team.Blue, SelectType.Pick)[0]);

        sut.SelectChamp(2);
        sut.NailDownChampion(new GameFlowData(GamePhase.Ban, Team.Red));
        Assert.AreEqual(2, storage.GetStorage(Team.Red, SelectType.Ban)[0]);
    }
}
