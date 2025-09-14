using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TraitConditionTests
{
    [Test]
    public void 방어력이_기준점_이하면_참()
    {
        var sut = new TraitConditionChecker();
        int threshold = 100;

        Assert.IsTrue(sut.CheckCondition(TraitConditionType.DefenseBelow, new ChampionStatData(0, 0, 0), threshold));
        Assert.IsTrue(sut.CheckCondition(TraitConditionType.DefenseBelow, new ChampionStatData(0, 100, 0), threshold));
        Assert.IsFalse(sut.CheckCondition(TraitConditionType.DefenseBelow, new ChampionStatData(0, 120, 0), threshold));
    }
}
