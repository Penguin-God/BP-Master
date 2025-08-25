using NUnit.Framework;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

public class StatManagementTests
{
    ChampionStatData CreateData(int atk, int def = 0, int range = 0, int speed = 0) => new ChampionStatData(atk, def, range, speed);

    [Test]
    public void 지정한_측과_인덱스만_변경()
    {
        // arrange
        var sut = new StatManager(
            self: new[] { CreateData(10), CreateData(20) },
            opponent: new[] { CreateData(40), CreateData(50) }
        );

        // act: 상대 측(index 1)의 공격력만 -12
        sut.ChangeSelectData(Side.Opponent, 1, c => new ChampionStatData(c.Attack - 12, c.Defense, c.Range, c.Speed));

        // assert: 타겟만 변경, 나머지는 그대로
        Assert.AreEqual(40, sut.Opponent[0].Attack);
        Assert.AreEqual(38, sut.Opponent[1].Attack);
        Assert.AreEqual(10, sut.Self[0].Attack);
        Assert.AreEqual(20, sut.Self[1].Attack);
    }

    [Test]
    public void 결과가_음수면_0()
    {
        // arrange
        var sut = new StatManager(
            self: new[] { CreateData(5) },
            opponent: Array.Empty<ChampionStatData>()
        );

        // act: -999 시도 → ChampionStatData 생성자에서 0으로 클램프
        sut.ChangeSelectData(Side.Self, 0, c => new ChampionStatData(c.Attack - 999, c.Defense, c.Range, c.Speed));

        // assert
        Assert.AreEqual(0, sut.Self[0].Attack);
    }

    [Test]
    public void 지정한_측의_모든_원소_변경()
    {
        // arrange
        var sut = new StatManager(
            self: new[] { CreateData(10), CreateData(20) },
            opponent: new[] { CreateData(30), CreateData(40) }
        );

        // act: Self 전원 공격력 -12 (음수는 구조체 생성자에서 0 클램프)
        sut.ChangeAll(Side.Self, c => new ChampionStatData(c.Attack - 12, c.Defense, c.Range, c.Speed));

        // assert
        Assert.AreEqual(0, sut.Self[0].Attack); // 10 -> 0
        Assert.AreEqual(8, sut.Self[1].Attack); // 20 -> 8
        Assert.AreEqual(30, sut.Opponent[0].Attack);
        Assert.AreEqual(40, sut.Opponent[1].Attack);
    }
}
