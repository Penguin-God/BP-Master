using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TraitExcuteTests
{
    ChampionStatData CreateData(int atk, int def = 0, int speed = 0) => new ChampionStatData(atk, def, speed);
    Trait CreateTrait(Side side, int amount) => new Trait(TraitType.Active, side, GetMinus(amount));

    [Test]
    public void 양팀_패시브_다_일괄_적용()
    {
        var statManager = new StatManager(blue: new[] { CreateData(50) }, red: new[] { CreateData(50) });
        PassiveExcutor sut = new(statManager,
            new Trait[] { new Trait(TraitType.Passive, Side.Opponent, GetMinus(20)) },
            new Trait[] { new Trait(TraitType.Passive, Side.Opponent, GetMinus(20)) }
            );

        sut.Do();
        Assert.AreEqual(30, statManager.Blue[0].Attack);
        Assert.AreEqual(30, statManager.Red[0].Attack);
    }

    [Test]
    public void 양팀_액티브_다_적용되면_끝()
    {
        var statManager = new StatManager(blue: new[] { CreateData(50) }, red: new[] { CreateData(50) });
        ActiveExcuteManager sut = new (
            new ActiveExcuter(statManager, Team.Blue, new Trait[] { CreateTrait(Side.Opponent, 20) }),
            new ActiveExcuter(statManager, Team.Red, new Trait[] { CreateTrait(Side.Opponent, 20) })
            );

        sut.DoActive(0, Team.Blue, new int[] { 0 });
        Assert.AreEqual(30, statManager.Red[0].Attack);
        Assert.IsTrue(sut.IsTeamDone(Team.Blue));
        Assert.IsFalse(sut.IsDone);
        sut.DoActive(0, Team.Red, new int[] { 0 });
        Assert.AreEqual(30, statManager.Blue[0].Attack);
        Assert.IsTrue(sut.IsDone);
        Assert.IsTrue(sut.IsTeamDone(Team.Red));
    }

    [Test]
    public void 액티브는_순서대로_적용되고_다_사용하면_끝()
    {
        var statManager = new StatManager(blue: new[] { CreateData(0) }, red: new[] { CreateData(50) });
        ActiveExcuter sut = new (statManager, Team.Blue, new Trait[] { CreateTrait(Side.Opponent, 20), CreateTrait(Side.Opponent, 25) });

        sut.DoActive(0, new int[] { 0 });
        Assert.AreEqual(30, statManager.Red[0].Attack);
        Assert.IsFalse(sut.IsDone);

        sut.DoActive(0, new int[] { 0 });
        Assert.AreEqual(5, statManager.Red[0].Attack);
        Assert.IsTrue(sut.IsDone);
    }

    [Test]
    public void 선택한_인덱스만_공격력_감소()
    {
        var result = new StatManager(
            blue: new[] { CreateData(0) },
            red: new[] { CreateData(40), CreateData(50) }
        );
        ActiveExcuter sut = new (result, Team.Blue, new Trait[] { CreateTrait(Side.Opponent, 20) });

        sut.DoActive(1, new int[] { 1 });

        Assert.AreEqual(40, result.Red[0].Attack);
        Assert.AreEqual(30, result.Red[1].Attack);
    }

    AttackMinus GetMinus(int amount) => new AttackMinus(amount);
}



public class AttackMinus : ITraitAction
{
    readonly int Amount;
    public AttackMinus(int amount) => Amount = amount;

    public ChampionStatData Do(ChampionStatData stat) => stat.ChangeAttack(stat.Attack - Amount);
}
