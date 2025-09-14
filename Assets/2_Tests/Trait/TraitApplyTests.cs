using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TraitApplyTests
{
    [Test]
    public void 조건_만족시_Action_실행됨()
    {
        var champion = TestHelper.CreateStatChamp(10, def: 60, 5);
        var action = new TestAttackChanger(5);
        var sut = new TraitExecutor(action, TraitConditionType.DefenseAtLeast, 50);

        sut.ExecteTrait(champion);

        Assert.AreEqual(15, champion.StatData.Attack);
    }

    [Test]
    public void 조건_불만족시_Action_실행되지않음()
    {
        var champion = TestHelper.CreateStatChamp(10, 40, 5);
        var action = new TestAttackChanger(5);
        var sut = new TraitExecutor(action, TraitConditionType.DefenseAtLeast, 50);

        sut.ExecteTrait(champion);

        Assert.AreEqual(10, champion.StatData.Attack);
    }
}
