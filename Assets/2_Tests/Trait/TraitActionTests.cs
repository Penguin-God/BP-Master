using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TraitActionTests
{
    [Test]
    public void 공_감소()
    {
        AttackWeaker sut = new(10);
        var data = CreateStat(12);

        var result = sut.Do(data);

        Assert.AreEqual(2, result.Attack);
    }

    ChampionStatData CreateStat(int att) => new ChampionStatData(att, 0, 0, 0);
}
