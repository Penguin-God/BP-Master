using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PassiveTests
{
    [Test]
    public void 공_감소()
    {
        AttackWeaker sut = new();
        var datas = new ChampionStatData[] { CreateStat(10), CreateStat(15) };

        ChampionStatData[] result = sut.Do(datas);

        Assert.AreEqual(0, result[0]);
        Assert.AreEqual(5, result[1]);
    }

    ChampionStatData CreateStat(int att) => new ChampionStatData(att, 0, 0, 0);
}
