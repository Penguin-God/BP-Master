using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PassiveExcuteTests
{
    ChampionStatData CreateData(int atk, int def = 0, int range = 0, int speed = 0) => new ChampionStatData(atk, def, range, speed);

    [Test]
    public void 액티브는_순서대로_적용되고_다_사용하면_끝()
    {
        var statManager = new StatManager(self: new[] { CreateData(0) }, opponent: new[] { CreateData(50) });
        ActiveExcuter sut = new ActiveExcuter(statManager, new Trait[] { new Trait(TraitType.Active, Side.Opponent, new AttackWeaker(20)), new Trait(TraitType.Active, Side.Opponent, new AttackWeaker(25)) });

        sut.DoActive(0);
        Assert.AreEqual(30, statManager.Opponent[0].Attack);
        Assert.IsFalse(sut.IsDone);

        sut.DoActive(0);
        Assert.AreEqual(5, statManager.Opponent[0].Attack);
        Assert.IsTrue(sut.IsDone);
    }

    [Test]
    public void 선택한_인덱스만_공격력_감소()
    {
        var result = new StatManager(
            self: new[] { CreateData(0) },
            opponent: new[] { CreateData(40), CreateData(50) }
        );
        ActiveExcuter sut = new ActiveExcuter(result, new Trait[] { new Trait(TraitType.Active, Side.Opponent, new AttackWeaker(20)) });

        sut.DoActive(1);

        Assert.AreEqual(40, result.Opponent[0].Attack);
        Assert.AreEqual(30, result.Opponent[1].Attack);
    }
}
