using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TraitActionTests
{
    [Test]
    [TestCase(10, 22)]
    [TestCase(-10, 2)]
    public void 공_변경(int amount, int expected)
    {
        AttackChanger sut = new(amount);
        var data = CreateStat(12);

        var result = sut.Do(data);

        Assert.AreEqual(expected, result.Attack);
    }

ChampionStatData CreateStat(int att) => new ChampionStatData(att, 0, 0);
}
