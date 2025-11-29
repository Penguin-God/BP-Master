using NUnit.Framework;
using static TestHelper;

public class AI_SimulateMainTests
{
    [Test]
    public void AI_배틀을_실행시키고_결과를_반환해야_함()
    {
        var eventDispatcher = new PhaseEventDispatcher();
        var phaseManager = CreatePhaseManager(eventDispatcher,
            CreatePhaseData(GamePhase.Ban, Team.Blue, Team.Red),
            CreatePhaseData(GamePhase.Pick, Team.Blue, Team.Red, Team.Red, Team.Blue),
            CreatePhaseData(GamePhase.Skill, Team.Blue, Team.Red, Team.Red, Team.Blue)
            );

        var storage = CreateStorage(1, 2, 3, 4, 5, 6);

        Champion[] champions = new Champion[] { CreateChampion(10), CreateChampion(20), CreateChampion(30), CreateChampion(100), CreateChampion(100), CreateChampion(60), };

        var blueSelector = new PrioritySelector(new int[] { 1 }, new int[] { 2, 3 });
        var blueBanPick = new AI_BanPickAgent(Team.Blue, phaseManager, storage, blueSelector, blueSelector);

        var redSelector = new PrioritySelector(new int[] { 4 }, new int[] { 5, 6 });
        var redBanPick = new AI_BanPickAgent(Team.Red, phaseManager, storage, redSelector, redSelector);


        SlotStorage<bool> skillUseStorage = new SlotStorage<bool>();
        skillUseStorage.AddSlot(Team.Blue, false);
        skillUseStorage.AddSlot(Team.Red, false);

        var filter = new SkillSlotFilter(skillUseStorage);
        var counter = new TargetCounter(teamSize: 2);

        SlotStorage<ChampionStatus> statusSlots = CreateTwoSlotStatus(att: 100);
        var skillController = new SkillUseController(statusSlots);
        skillController.OnUseSkill += slot => phaseManager.SubmitAction(slot.Team);

        SlotStorage<Skill> skillSlots = new SlotStorage<Skill>();
        skillSlots.AddSlot(Team.Blue, CreateValueSkill(SkillType.AttackChanger, 10, default, SelfAllRule));
        skillSlots.AddSlot(Team.Red, CreateValueSkill(SkillType.AttackChanger, 30, default, SelfAllRule));
        skillSlots.AddSlot(Team.Blue, CreateValueSkill(SkillType.AttackChanger, 10, default, SelfAllRule));
        skillSlots.AddSlot(Team.Red, CreateValueSkill(SkillType.AttackChanger, 30, default, SelfAllRule));

        var blueSkill = new AI_SkillAgent(Team.Blue, filter, skillSlots, skillController, counter);
        var redSkill = new AI_SkillAgent(Team.Red, filter, skillSlots, skillController, counter);

        var blue = new AI_PhaseAgent(blueBanPick, blueSkill);
        var red = new AI_PhaseAgent(redBanPick, redSkill);

        var sut = new BattleMain(phaseManager, eventDispatcher, champions, blue, red);

        MatchInfo result = sut.Run();

        Assert.AreEqual(Team.Red, result.Winner);
        Assert.AreEqual(380, result.BlueScore);
        Assert.AreEqual(600, result.RedScore);
    }

    Champion CreateChampion(int skillValue) => new Champion(CreateValueSkill(SkillType.AttackChanger, 10, default, SelfAllRule), CreateStatus(att: 100));
}
