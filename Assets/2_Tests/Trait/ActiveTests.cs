using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ActiveTests
{
    ChampionStatData CreateData(int atk, int def = 0, int speed = 0) => new ChampionStatData(atk, def, speed);
    Trait CreateTrait(Side side, int amount) => new Trait(TraitType.Active, side, new TestAttackChanger(amount));

    public class TestAttackChanger : ITraitAction
    {
        readonly int Amount;
        public TestAttackChanger(int amount) => Amount = amount;

        public ChampionStatData Do(ChampionStatData stat) => stat.ChangeAttack(stat.Attack + Amount);
    }


    [Test]
    public void 각_팀_챔피언별로_스킬_사용_가능()
    {
        var statManager = new StatManager(
            blue: new[] { CreateData(50), CreateData(60), CreateData(70) },
            red: new[] { CreateData(40), CreateData(50), CreateData(60) }
        );

        // Blue팀: 3명의 챔피언, 각각 상대방 -10 공격력 감소 스킬
        var blueTraits = new Trait[]
        {
            CreateTrait(Side.Opponent, -10), // 챔피언0의 스킬
            CreateTrait(Side.Opponent, -15), // 챔피언1의 스킬  
            CreateTrait(Side.Opponent, -20)  // 챔피언2의 스킬
        };

        ActiveExcuter sut = new ActiveExcuter(statManager, Team.Blue, blueTraits);

        sut.DoActive(0);
        Assert.AreEqual(30, statManager.Red[0].Attack); // 40 - 10 = 30
        Assert.IsTrue(sut.IsChampionUsed(0));
        Assert.IsFalse(sut.IsTeamDone());

        sut.DoActive(1);
        Assert.AreEqual(35, statManager.Red[1].Attack); // 50 - 15 = 35
        Assert.IsTrue(sut.IsChampionUsed(1));
        Assert.IsFalse(sut.IsTeamDone());

        sut.DoActive(2);
        Assert.AreEqual(40, statManager.Red[2].Attack); // 60 - 20 = 40
        Assert.IsTrue(sut.IsChampionUsed(2));
        Assert.IsTrue(sut.IsTeamDone()); // 모든 챔피언 사용 완료
    }

    [Test]
    public void 이미_사용한_챔피언_스킬은_재사용_불가()
    {
        var statManager = new StatManager(
            blue: new[] { CreateData(50) },
            red: new[] { CreateData(40) }
        );

        var blueTraits = new Trait[] { CreateTrait(Side.Opponent, -10) };
        ActiveExcuter sut = new ActiveExcuter(statManager, Team.Blue, blueTraits);

        sut.DoActive(0);
        Assert.AreEqual(30, statManager.Red[0].Attack);
        Assert.IsTrue(sut.IsChampionUsed(0));

        // 같은 챔피언 재사용 시도 - 효과 없어야 함
        sut.DoActive(0);
        Assert.AreEqual(30, statManager.Red[0].Attack); // 변화 없음
    }

    [Test]
    public void ActiveExcuteManager_양팀_모두_완료시_끝남()
    {
        var statManager = new StatManager(
            blue: new[] { CreateData(50), CreateData(60) },
            red: new[] { CreateData(40), CreateData(50) }
        );

        var blueExcuter = new ActiveExcuter(statManager, Team.Blue, new Trait[] { CreateTrait(Side.Opponent, -5)});
        var redExcuter = new ActiveExcuter(statManager, Team.Red, new Trait[] { CreateTrait(Side.Opponent, -5)});

        ActiveExcuteManager sut = new ActiveExcuteManager(blueExcuter, redExcuter);

        sut.DoActive(0, Team.Blue);
        Assert.IsFalse(sut.IsDone);

        sut.DoActive(0, Team.Red);
        Assert.IsTrue(sut.IsDone);
    }

    [Test]
    public void 아군_전체_대상_스킬_적용()
    {
        var statManager = new StatManager(
            blue: new[] { CreateData(50), CreateData(60) },
            red: new[] { CreateData(40), CreateData(50) }
        );

        // Blue팀 챔피언0이 아군 전체 공격력 +5 스킬 보유
        var blueTraits = new Trait[]
        {
            new Trait(TraitType.Active, Side.Self, new TestAttackChanger(5)),
            CreateTrait(Side.Opponent, -10)
        };

        ActiveExcuter sut = new ActiveExcuter(statManager, Team.Blue, blueTraits);

        sut.DoActive(0); // 아군 전체 버프

        Assert.AreEqual(55, statManager.Blue[0].Attack); // 50 + 5
        Assert.AreEqual(65, statManager.Blue[1].Attack); // 60 + 5
        Assert.AreEqual(40, statManager.Red[0].Attack); // 변화 없음
        Assert.AreEqual(50, statManager.Red[1].Attack); // 변화 없음
    }
}
