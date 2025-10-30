using NUnit.Framework;

public class SelectPresentTests
{
    [Test]
    public void 챔피언_선택_후_확정()
    {
        GameBanPickStorage storage = new(new int[] { 1, 2, 3 });
        ChampionSelectPresenter sut = new(storage);
        int ban = 0;
        int pick = 0;
        storage.OnBan += (team, id) => ban = id;
        storage.OnPick += (slot, id) => pick = id;


        sut.SelectChamp(1);
        sut.NailDownChampion(new GameFlowData(GamePhase.Pick, Team.Blue));
        Assert.AreEqual(1, pick);

        sut.SelectChamp(2);
        sut.NailDownChampion(new GameFlowData(GamePhase.Ban, Team.Red));
        Assert.AreEqual(2, ban);
    }
}
