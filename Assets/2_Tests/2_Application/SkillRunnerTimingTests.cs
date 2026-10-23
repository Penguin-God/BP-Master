using NUnit.Framework;
using static TestHelper;
using System;

[TestFixture]
public class SkillRunnerTimingTests
{
    BanPickEventDispatcher bpDispatcher;
    PhaseEventDispatcher phaseDispatcher;
    ChampionStatus caster;
    ChampionStatus target;

    [SetUp]
    public void SetUp()
    {
        bpDispatcher = new BanPickEventDispatcher();
        phaseDispatcher = new PhaseEventDispatcher();
        caster = CreateStatus(100, 100, 100);
        target = CreateStatus(100, 100, 100);
    }

    SkillRunner CreateSut() => new SkillRunner(new SkillActionFactory(bpDispatcher, phaseDispatcher));

    Skill CreateAttackSkill(SkillUseTimingType timing)
    {
        var data = new SkillData(
            SkillType.StatChanger,
            CalculatorFactory.CreateValue(20),
            SkillCondtionFactory.CreateNull(),
            default,
            SkillApplyType.Select,
            SkillValueType.Value,
            StatType.Attack,
            timing
        );
        return TestHelper.CreateSkill(data);
    }

    Skill CreateSkill(SkillUseTimingType timing, SkillApplyType applyType, int amount)
    {
        var data = new SkillData(
            SkillType.StatChanger,
            CalculatorFactory.CreateValue(amount),
            SkillCondtionFactory.CreateNull(),
            default,
            applyType,
            SkillValueType.Value,
            StatType.Attack,
            timing
        );
        return TestHelper.CreateSkill(data);
    }

    [Test]
    public void 즉시_발동_스킬은_Run_호출_시_스탯이_즉시_적용된다()
    {
        var sut = CreateSut();

        sut.Run(CreateAttackSkill(SkillUseTimingType.Immediate), caster, new[] { target }, Team.Blue);

        Assert.AreEqual(120, target.Stat.Attack);
    }

    [Test]
    public void 페이즈_종료_스킬은_Run_직후에는_적용되지_않고_페이즈_완료_이벤트_시_적용된다()
    {
        var sut = CreateSut();

        sut.Run(CreateAttackSkill(SkillUseTimingType.OnPhaseDone), caster, new[] { target }, Team.Blue);
        Assert.AreEqual(100, target.Stat.Attack);

        phaseDispatcher.Dispatch(GamePhase.Done, Team.All);
        Assert.AreEqual(120, target.Stat.Attack);
    }

    [Test]
    public void 아군_픽_순간_자신에게_스킬_적용()
    {
        var sut = CreateSut();
        var caster = CreateStatus(100, 100, 100);
        var skill = CreateSkill(SkillUseTimingType.OnSelfPick, SkillApplyType.OneSelf, 25);

        sut.Run(skill, caster, Array.Empty<ChampionStatus>(), Team.Blue);
        bpDispatcher.RaisePick(CreateChampion(), new SlotData(Team.Blue, 0));

        Assert.AreEqual(125, caster.Stat.Attack);
    }

    [Test]
    public void 적군_픽_순간_그_카드에게_스킬_적용()
    {
        var sut = CreateSut();
        var caster = CreateStatus(100, 100, 100);
        var skill = CreateSkill(SkillUseTimingType.OnOpponentPick, SkillApplyType.Triggered, -20);

        sut.Run(skill, caster, Array.Empty<ChampionStatus>(), Team.Blue);

        var opponentChampion = CreateChampion(att:100);
        bpDispatcher.RaisePick(opponentChampion, new SlotData(Team.Red, 0));

        Assert.AreEqual(80, opponentChampion.Status.Stat.Attack);
    }
}