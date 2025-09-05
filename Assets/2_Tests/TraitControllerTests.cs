using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TraitControllerTests
{
    [Test]
    public void 특성_적용()
    {
        TraitController sut = new TraitController();
        Champion champion = new Champion(0, "", default, null);

        sut.ApplyTrait(new TestAttackChanger(10), new Champion[] { champion });

        Assert.AreEqual(10, champion.StatData.Attack);
    }
}
