using NUnit.Framework;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

public class StatChangeTests
{
    ChampionStatData CreateData(int atk, int def = 0, int range = 0, int speed = 0) => new ChampionStatData(atk, def, range, speed);

    [Test]
    public void 지정한_측과_인덱스만_변경()
    {
        var sut = new StatManager(
            blue: new[] { CreateData(10), CreateData(20) },
            red: new[] { CreateData(40), CreateData(50) }
        );

        sut.ChangeSelectData(Team.Red, 1, c => new ChampionStatData(c.Attack - 12, c.Defense, c.Range, c.Speed));

        // assert: 타겟만 변경, 나머지는 그대로
        Assert.AreEqual(40, sut.Red[0].Attack);
        Assert.AreEqual(38, sut.Red[1].Attack);
        Assert.AreEqual(10, sut.Blue[0].Attack);
        Assert.AreEqual(20, sut.Blue[1].Attack);
    }

    [Test]
    public void 결과가_음수면_0()
    {
        var sut = new StatManager(
            blue: new[] { CreateData(5) },
            red: Array.Empty<ChampionStatData>()
        );

        // act: -999 시도
        sut.ChangeSelectData(Team.Blue, 0, c => new ChampionStatData(c.Attack - 999, c.Defense, c.Range, c.Speed));

        Assert.AreEqual(0, sut.Blue[0].Attack);
    }

    [Test]
    public void 지정한_측의_모든_원소_변경()
    {
        var sut = new StatManager(
            blue: new[] { CreateData(10), CreateData(20) },
            red: new[] { CreateData(30), CreateData(40) }
        );

        sut.ChangeAll(Team.Blue, c => new ChampionStatData(c.Attack - 12, c.Defense, c.Range, c.Speed));

        // assert
        Assert.AreEqual(0, sut.Blue[0].Attack); // 10 -> 0
        Assert.AreEqual(8, sut.Blue[1].Attack); // 20 -> 8
        Assert.AreEqual(30, sut.Red[0].Attack);
        Assert.AreEqual(40, sut.Red[1].Attack);
    }
}
