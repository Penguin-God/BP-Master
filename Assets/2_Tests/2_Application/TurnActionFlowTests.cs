using NUnit.Framework;
using static TestHelper;

public class TurnActionFlowTests
{
    SlotStorage<ChampionStatus> CreateSut(params (Team team, int att, int def)[] cards)
    {
        var storage = new SlotStorage<ChampionStatus>();
        foreach (var card in cards)
            storage.AddSlot(card.team, CreateStatus(card.att, card.def));
        return storage;
    }

    [Test]
    public void 상대_진영에_살아있는_카드가_없으면_공격_스킵_조건이_참이_된다()
    {
        var sut = CreateSut((Team.Blue, 10, 20), (Team.Red, 15, 0), (Team.Red, 10, -5));

        var result = TurnActionFlowHelper.ShouldSkipAttack(Team.Red, sut);

        Assert.IsTrue(result);
    }

    [TestCase(1)]
    [TestCase(25)]
    public void 상대_진영에_살아있는_카드가_있으면_공격_스킵_조건이_거짓이_된다(int opponentDefense)
    {
        var sut = CreateSut((Team.Blue, 10, 20), (Team.Red, 15, opponentDefense));

        var result = TurnActionFlowHelper.ShouldSkipAttack(Team.Red, sut);

        Assert.IsFalse(result);
    }

    [TestCase(0, true)]
    [TestCase(1, false)]
    public void 스킬_데이터_유무에_따라_스킬_스킵_조건이_올바르게_판정된다(int skillCount, bool expectedSkip)
    {
        var sut = skillCount == 0
            ? CreateSkill()
            : CreateSkill(CreateConditionFreeSkill(StatType.Attack, 10, SelfAllRule));

        var result = TurnActionFlowHelper.ShouldSkipSkill(sut);

        Assert.AreEqual(expectedSkip, result);
    }

    [Test]
    public void 스킬로_공격력이_증가한_후_공격_단계가_실행되어_증가된_공격력만큼_상대_방어력이_차감된다()
    {
        var facade = new PickSlotFacade();
        var skillData = CreateConditionFreeSkill(StatType.Attack, 10, SelfAllRule);
        facade.Add(Team.Blue, CreateChampion(id: 1, att: 15, def: 30, skillData: skillData));
        facade.Add(Team.Red, CreateChampion(id: 2, att: 10, def: 30));
        var sut = new SkillUsecase(facade.ChampionSlots, CreateSkillRunner());

        sut.UseSkill(BlueZeroSlot, new[] { BlueZeroSlot });
        CardAttackService.AttackFirstAliveTarget(BlueZeroSlot, facade.StatusSlots);

        Assert.AreEqual(25, facade.StatusSlots.GetSlot(BlueZeroSlot).Stat.Attack);
        Assert.AreEqual(5, facade.StatusSlots.GetSlot(RedZeroSlot).Stat.Defense);
    }
}
