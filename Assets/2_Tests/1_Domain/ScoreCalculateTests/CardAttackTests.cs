using NUnit.Framework;
using static TestHelper;

public class CardAttackTests
{
    ChampionStatus CreateSut(int attack, int defense) => CreateStatus(attack, defense);

    [TestCase(10, 30, 20)]
    [TestCase(25, 25, 0)]
    [TestCase(0, 15, 15)]
    public void 공격자의_공격력만큼_피격자의_방어력이_차감된다(int attackerAttack, int targetDefense, int expectedDefense)
    {
        var attacker = CreateSut(attackerAttack, 20);
        var sut = CreateSut(10, targetDefense);

        CardAttackService.ExecuteAttack(attacker, sut);

        Assert.AreEqual(expectedDefense, sut.Stat.Defense);
    }

    [TestCase(10, 20, false)]
    [TestCase(20, 20, true)]
    [TestCase(30, 20, true)]
    public void 방어력이_0_이하가_된_카드는_사망_상태가_된다(int damage, int initialDefense, bool expectedIsDead)
    {
        var sut = CreateSut(10, initialDefense);

        sut.TakeDamage(damage);

        Assert.AreEqual(expectedIsDead, sut.IsDead);
    }

    [TestCase(30, 20)]
    [TestCase(100, 10)]
    public void 피격자의_방어력이_공격력보다_낮아도_방어력이_음수가_되지_않고_0으로_고정된다(int attackerAttack, int targetDefense)
    {
        var attacker = CreateSut(attackerAttack, 10);
        var sut = CreateSut(10, targetDefense);

        CardAttackService.ExecuteAttack(attacker, sut);

        Assert.AreEqual(0, sut.Stat.Defense);
        Assert.IsTrue(sut.IsDead);
    }

    [Test]
    public void 상대_진영에_살아있는_카드가_없을_때_공격_대상_목록이_비어_있다()
    {
        var sut = new SlotStorage<ChampionStatus>();
        sut.AddSlot(Team.Blue, CreateSut(20, 20));
        sut.AddSlot(Team.Red, CreateSut(15, 0));
        sut.AddSlot(Team.Red, CreateSut(10, -5));

        var result = CardAttackService.GetAliveTargetSlots(Team.Red, sut);

        CollectionAssert.IsEmpty(result);
    }

    [Test]
    public void 상대_진영에_생존_카드가_있으면_첫_번째_생존_카드를_타격한다()
    {
        var sut = new SlotStorage<ChampionStatus>();
        sut.AddSlot(Team.Blue, CreateSut(15, 20));
        sut.AddSlot(Team.Red, CreateSut(10, 0));
        sut.AddSlot(Team.Red, CreateSut(10, 25));

        CardAttackService.AttackFirstAliveTarget(BlueZeroSlot, sut);

        Assert.AreEqual(10, sut.GetSlot(RedOneSlot).Stat.Defense);
    }

    [Test]
    public void 사망한_카드는_생존_점수_계산에서_완전히_제외된다()
    {
        var sut = new SlotStorage<ChampionStatus>();
        sut.AddSlot(Team.Blue, CreateSut(50, 0));
        sut.AddSlot(Team.Blue, CreateSut(20, 30));
        sut.AddSlot(Team.Red, CreateSut(100, 0));
        sut.AddSlot(Team.Red, CreateSut(10, 15));

        var result = SurvivalScoreCalculator.Calculate(sut);

        Assert.AreEqual(20, result.BlueInfo.AttackTotal);
        Assert.AreEqual(30, result.BlueInfo.DefenseTotal);
        Assert.AreEqual(50, result.BlueInfo.Total);
        Assert.AreEqual(10, result.RedInfo.AttackTotal);
        Assert.AreEqual(15, result.RedInfo.DefenseTotal);
        Assert.AreEqual(25, result.RedInfo.Total);
    }

    [TestCase(30, 20, 10, 20, Team.Blue)]
    [TestCase(10, 10, 25, 20, Team.Red)]
    [TestCase(20, 20, 15, 25, Team.All)]
    public void 양_팀_생존_카드의_공방_점수에_따라_올바른_승리_팀이_판정된다(int blueAtt, int blueDef, int redAtt, int redDef, Team expectedWinner)
    {
        var sut = new SlotStorage<ChampionStatus>();
        sut.AddSlot(Team.Blue, CreateSut(blueAtt, blueDef));
        sut.AddSlot(Team.Red, CreateSut(redAtt, redDef));

        var result = SurvivalScoreCalculator.Calculate(sut);

        Assert.AreEqual(expectedWinner, result.Winner);
    }
}
