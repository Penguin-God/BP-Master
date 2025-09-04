using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class SelectPresentTests
{
    [Test]
    public void 프레젠터_선택_대행()
    {
        GameBanPickStorage storage = new(new int[] { 1, 2, 3 });
        SelectPresenter sut = new(storage);

        sut.ChangeFlow(new GameFlowData(GamePhase.Pick, Team.Blue));
        sut.SelectChamp(1);
        //Assert.IsTrue(sut.NailDownChampion(1));
        Assert.AreEqual(1, storage.GetStorage(Team.Blue, SelectType.Pick)[0]);

        sut.ChangeFlow(new GameFlowData(GamePhase.Ban, Team.Red));
        sut.SelectChamp(2);
        Assert.AreEqual(2, storage.GetStorage(Team.Red, SelectType.Ban)[0]);

        sut.ChangeFlow(new GameFlowData(GamePhase.Swap, Team.Blue));
        sut.SelectChamp(3);
        //Assert.IsFalse(sut.NailDownChampion(3));
    }
}
