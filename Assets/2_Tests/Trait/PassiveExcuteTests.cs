using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PassiveExcuteTests
{
    ChampionStatData CreateData(int atk, int def = 0, int range = 0, int speed = 0) => new ChampionStatData(atk, def, range, speed);

    [Test]
    public void 액티브는_다_사용하면_끝()
    {
        //ActiveExcuter sut = new(new IActivePassive[] { new SelectAttackWeaker(0), new SelectAttackWeaker(0) });

        //sut.Do(0);
        //Assert.IsFalse(sut.IsDone);

        //sut.Do(0);
        //Assert.IsTrue(sut.IsDone);
    }

    [Test]
    public void 선택한_인덱스만_공격력_감소()
    {
        var result = new StatManager(
            self: new[] { CreateData(0) },
            opponent: new[] { CreateData(40), CreateData(50) }
        );
        ActiveExcuter sut = new ActiveExcuter(result, new ITraitAction[] { new AttackWeaker(20) });

        sut.DoActive(1);

        Assert.AreEqual(40, result.Opponent[0].Attack);
        Assert.AreEqual(30, result.Opponent[1].Attack);
    }
}
