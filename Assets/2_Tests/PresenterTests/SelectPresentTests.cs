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
        SelectPresenter sut = new();

        Assert.IsTrue(sut.SelectChampion(GamePhase.Pick, Team.Blue, 1));
        Assert.AreEqual(1, storage.GetStorage(Team.Blue, SelectType.Pick));

        Assert.IsTrue(sut.SelectChampion(GamePhase.Ban, Team.Red, 2));
        Assert.AreEqual(1, storage.GetStorage(Team.Red, SelectType.Ban));

        Assert.IsFalse(sut.SelectChampion(GamePhase.Swap, 3));
    }
}
