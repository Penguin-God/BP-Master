using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class SelectPresentTests
{
    [Test]
    public void 챔피언_선택_후_확정()
    {
        GameBanPickStorage storage = new(new int[] { 1, 2, 3 });
        ChampionSelectPresenter sut = new(storage);

        sut.ChangeFlow(new GameFlowData(GamePhase.Pick, Team.Blue));
        sut.SelectChamp(1);
        Assert.AreEqual(1, sut.NailDownChampion());
        Assert.AreEqual(1, storage.GetStorage(Team.Blue, SelectType.Pick)[0]);

        sut.ChangeFlow(new GameFlowData(GamePhase.Ban, Team.Red));
        sut.SelectChamp(2);
        Assert.AreEqual(2, sut.NailDownChampion());
        Assert.AreEqual(2, storage.GetStorage(Team.Red, SelectType.Ban)[0]);

        sut.SelectChamp(55);
        Assert.AreEqual(-1, sut.NailDownChampion());
    }
}
