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
        var phaseManager = CreatePhaseManager(eventDispatcher,
            CreatePhaseData(GamePhase.Ban, Team.Blue, Team.Red),
            CreatePhaseData(GamePhase.Pick, Team.Blue, Team.Red, Team.Red, Team.Blue)
            );
        var sut = new AI_BattleSimulator(phaseManager, eventDispatcher);

        SlotStorage<bool> skillUseStorage = new SlotStorage<bool>();
        skillUseStorage.AddSlot(Team.Blue, false);
        skillUseStorage.AddSlot(Team.Red, false);
        var filter = new SkillSlotFilter(skillUseStorage);
        var counter = new TargetCounter(teamSize: 1);

        // 이거 설정해야됨
        SlotStorage<ChampionStatus> statusSlots = new SlotStorage<ChampionStatus>();
        
        var blue = new AI_SkillAgent(Team.Blue, filter, null, null, counter);

        var red = new AI_SkillAgent(Team.Red, filter, null, null, counter);

        sut.RunSkill(blue, red);
    }
}
