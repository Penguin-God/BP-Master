using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

public class AI_AgentTests
{
    // Fake
    class FirstBan : IBanSelector
    {
        public int Ban(HashSet<int> ids) => ids.First();
    }
    class FirstPick : IPickSelector
    {
        public int Pick(HashSet<int> ids) => ids.First();
    }

    PhaseManager CreatePhaseManager(GamePhase phase, params Team[] turns)
    {
        var dispatcher = new PhaseEventDispatcher();
        var phases = new[]
        {
            new PhaseData(phase, new Phase(turns.ToArray()))
        };
        var pm = new PhaseManager(phases, dispatcher);
        pm.Start(); // CurrentFlow 설정
        return pm;
    }

    AI_SelectAgent CreateFirstSelectSut(Team team, PhaseManager pm, GameBanPickStorage storage) => new AI_SelectAgent(team, pm, storage, new FirstBan(), new FirstPick());

    [Test]
    public void 규칙에_맞게_밴_선택_후_저장_및_턴_진행()
    {
        var storage = new GameBanPickStorage(new[] { 3, 7, 9 });
        var phaseManager = CreatePhaseManager(GamePhase.Ban, Team.Blue, Team.Blue, Team.Red);
        var sut = CreateFirstSelectSut(Team.Blue, phaseManager, storage);

        sut.Ban(Team.Blue);
        sut.Ban(Team.Blue);

        CollectionAssert.AreEqual(new[] { 3, 7 }, storage.GetStorage(Team.Blue, SelectType.Ban));
        Assert.IsFalse(storage.SelectableIds.Contains(3));
        Assert.IsFalse(storage.SelectableIds.Contains(7));
        Assert.AreEqual(Team.Red, phaseManager.CurrentTurn);
    }

    [Test]
    public void 규칙에_맞게_픽_선택_후_저장_및_턴_진행()
    {
        var storage = new GameBanPickStorage(new[] { 2, 5, 8 });
        var phaseManager = CreatePhaseManager(GamePhase.Pick, Team.Red, Team.Blue);
        var sut = CreateFirstSelectSut(Team.Red, phaseManager, storage);

        sut.Pick(Team.Red);

        var picked = storage.GetStorage(Team.Red, SelectType.Pick);
        CollectionAssert.AreEqual(new[] { 2 }, picked);
        Assert.IsFalse(storage.SelectableIds.Contains(2));
        Assert.AreEqual(Team.Blue, phaseManager.CurrentTurn);
    }

    [Test]
    public void 자기_팀_차례가_아니면_아무_일도_일어나지_않음()
    {
        var storage = new GameBanPickStorage(new[] { 1, 4, 6 });
        var phaseManager = CreatePhaseManager(GamePhase.Ban, Team.Blue);
        var sut = CreateFirstSelectSut(Team.Blue, phaseManager, storage);

        // 팀 불일치
        sut.Ban(Team.Red);
        sut.Pick(Team.Red);

        CollectionAssert.IsEmpty(storage.GetStorage(Team.Blue, SelectType.Ban));
        CollectionAssert.IsEmpty(storage.GetStorage(Team.Blue, SelectType.Pick));
    }
}
