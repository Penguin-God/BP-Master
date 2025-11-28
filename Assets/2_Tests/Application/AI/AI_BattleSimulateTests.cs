using NUnit.Framework;
using static TestHelper;

public class AI_BattleSimulateTests
{
    [Test]
    public void 주입한_AI로_밴픽_진행해_저장()
    {
        var eventDispatcher = new PhaseEventDispatcher();
        var phaseManager = CreatePhaseManager(eventDispatcher,
            CreatePhaseData(GamePhase.Ban, Team.Blue, Team.Red),
            CreatePhaseData(GamePhase.Pick, Team.Blue, Team.Red, Team.Red, Team.Blue)
            );
        var sut = new AI_BattleSimulator(phaseManager, eventDispatcher);
        var storage = CreateStorage(1, 2, 3, 4, 5, 6);

        var blueSelector = new PrioritySelector(new int[] { 1 }, new int[] { 2, 3 });
        var blue = new AI_BanPickAgent(Team.Blue, phaseManager, storage, blueSelector, blueSelector);

        var redSelector = new PrioritySelector(new int[] { 4 }, new int[] { 5, 6 });
        var red = new AI_BanPickAgent(Team.Red, phaseManager, storage, redSelector, redSelector);

        sut.RunBanPick(blue, red);

        CollectionAssert.AreEqual(storage.BanStorage[Team.Blue], new int[] { 1 });
        CollectionAssert.AreEqual(storage.BanStorage[Team.Red], new int[] { 4 });

        CollectionAssert.AreEqual(storage.PickIds.GetTeam(Team.Blue), new int[] { 2, 3 });
        CollectionAssert.AreEqual(storage.PickIds.GetTeam(Team.Red), new int[] { 5, 6 });
    }

    [Test]
    public void 주입한_AI로_스킬_진행()
    {
        var eventDispatcher = new PhaseEventDispatcher();
        var phaseManager = CreatePhaseManager(eventDispatcher, CreatePhaseData(GamePhase.Skill, Team.Blue, Team.Red));
        var sut = new AI_BattleSimulator(phaseManager, eventDispatcher);

        SlotStorage<bool> skillUseStorage = new SlotStorage<bool>();
        skillUseStorage.AddSlot(Team.Blue, false);
        skillUseStorage.AddSlot(Team.Red, false);

        var filter = new SkillSlotFilter(skillUseStorage);
        var counter = new TargetCounter(teamSize: 1);

        SlotStorage<ChampionStatus> statusSlots = CreateOneSlotStatus(att: 100);
        var skillController = new SkillUseController(statusSlots);
        skillController.OnUseSkill += slot => phaseManager.SubmitAction(slot.Team);

        SlotStorage<Skill> skillSlots = new SlotStorage<Skill>();
        skillSlots.AddSlot(Team.Blue, CreateValueSkill(SkillType.AttackChanger, 10, default, SelfAllRule));
        skillSlots.AddSlot(Team.Red, CreateValueSkill(SkillType.AttackChanger, 30, default, SelfAllRule));

        var blue = new AI_SkillAgent(Team.Blue, filter, skillSlots, skillController, counter);
        var red = new AI_SkillAgent(Team.Red, filter, skillSlots, skillController, counter);

        sut.RunSkill(blue, red);
        phaseManager.Start();

        Assert.AreEqual(110, statusSlots.GetSlot(BlueZeroSlot).Stat.Attack);
        Assert.AreEqual(130, statusSlots.GetSlot(RedZeroSlot).Stat.Attack);
    }
}
