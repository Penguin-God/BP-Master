using NUnit.Framework;
using static TestHelper;

public class MatchCoreTests
{
    [Test]
    public void 매치코어_생성시_퍼블릭_의존성_객체들이_초기화된다()
    {
        var blueMastery = new MasteryStatCollection(new ChampionMastery[0]);
        var redMastery = new MasteryStatCollection(new ChampionMastery[0]);

        var matchCore = new MatchCore(CreateCaltalog(), CreateStorage(2), CreatePhaseAdvancer(), blueMastery, redMastery);

        var blueEntry = new DummyPhaseEntry(Team.Blue);
        var redEntry = new DummyPhaseEntry(Team.Red);
        matchCore.SetupPhaseManager(blueEntry, redEntry);

        Assert.IsNotNull(matchCore.PhaseManager);
        Assert.IsNotNull(matchCore.BanPickHandler);
        Assert.IsNotNull(matchCore.SkillController);
        Assert.IsNotNull(matchCore.MasteryRegistry);
        Assert.IsNotNull(matchCore.PhaseEventDispatcher);
        Assert.IsNotNull(matchCore.PhaseAdvancer);
    }

    class DummyPhaseEntry : IPhaseEntry
    {
        public Team Team { get; }
        public DummyPhaseEntry(Team team) => Team = team;
        public void EnterBan() { }
        public void EnterPick() { }
    }
}