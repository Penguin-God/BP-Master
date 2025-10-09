using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

public class AI_AgentTests
{
    // Fake
    class MinSelector : IAI_Selector
    {
        public int Ban(HashSet<int> ids) => ids.Min();
        public int Pick(HashSet<int> ids) => ids.Min();
    }

    private static PhaseManager CreatePhaseManager(GamePhase phase, params Team[] turns)
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

    [Test]
    public void 밴을_적절한_저장소에_저장()
    {
        // Arrange
        var storage = new GameBanPickStorage(new[] { 3, 7, 9 });
        var phaseManager = CreatePhaseManager(GamePhase.Ban, Team.Blue);
        var sut = new AI_SelectAgent(Team.Blue, phaseManager, storage, new MinSelector());

        // Act
        sut.Select(Team.Blue);

        // Assert
        // MinSelector이므로 3이 밴됨
        CollectionAssert.AreEqual(new[] { 3 }, storage.GetStorage(Team.Blue, SelectType.Ban));
        Assert.IsFalse(storage.SelectableIds.Contains(3));
    }

    [Test]
    public void 픽을_적절한_저장소에_저장()
    {
        // Arrange
        var storage = new GameBanPickStorage(new[] { 2, 5, 8 });
        var phaseManager = CreatePhaseManager(GamePhase.Pick, Team.Red);
        var sut = new AI_SelectAgent(Team.Red, phaseManager, storage, new MinSelector());

        // Act
        sut.Select(Team.Red);

        // Assert
        // MinSelector이므로 2가 픽됨
        var picked = storage.GetStorage(Team.Red, SelectType.Pick);
        CollectionAssert.AreEqual(new[] { 2 }, picked);
        Assert.IsFalse(storage.SelectableIds.Contains(2));
    }

    [Test]
    public void 선택시_턴_진행()
    {
        // Arrange
        var storage = new GameBanPickStorage(new int[] { 3 });
        var phaseManager = CreatePhaseManager(GamePhase.Pick, Team.Red, Team.Blue);
        var sut = new AI_SelectAgent(Team.Red, phaseManager, storage, new MinSelector());

        // Act
        sut.Select(Team.Red);

        // Assert
        Assert.AreEqual(Team.Blue, phaseManager.CurrentTurn);
    }

    [Test]
    public void 자기_팀_차례가_아니면_아무_일도_일어나지_않음()
    {
        // Arrange
        var storage = new GameBanPickStorage(new[] { 1, 4, 6 });
        var phaseManager = CreatePhaseManager(GamePhase.Ban, Team.Blue);
        var sut = new AI_SelectAgent(Team.Blue, phaseManager, storage, new MinSelector());

        // Act
        sut.Select(Team.Red); // 팀 불일치 → 조기 반환

        // Assert
        CollectionAssert.IsEmpty(storage.GetStorage(Team.Blue, SelectType.Ban));
        CollectionAssert.IsEmpty(storage.GetStorage(Team.Blue, SelectType.Pick));
    }
}
