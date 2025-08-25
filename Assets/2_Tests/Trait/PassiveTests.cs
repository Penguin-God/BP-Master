using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PassiveTests
{
    [Test]
    public void 공_감소()
    {
        AttackWeaker sut = new(12);
        var datas = new ChampionStatData[] { CreateStat(10), CreateStat(15) };

        ChampionStatData[] result = sut.Do(datas);

        Assert.AreEqual(0, result[0].Attack);
        Assert.AreEqual(3, result[1].Attack);
    }

    [Test]
    public void 선택한_인덱스만_공격력_감소()
    {
        // arrange
        var sut = new AttackWeaker(12);
        var datas = new[]
        {
            CreateStat(10), // 그대로 10
            CreateStat(15), // 15 -> 3 (감소 대상)
        };

        // act
        var result = sut.DoAtIndex(datas, targetIndex: 1);

        // assert
        Assert.AreEqual(10, result[0].Attack);
        Assert.AreEqual(3, result[1].Attack);
    }

    ChampionStatData CreateStat(int att) => new ChampionStatData(att, 0, 0, 0);
}
