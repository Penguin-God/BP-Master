using NUnit.Framework;
using static TestHelper;

public class MatchCoreTests
{
    [Test]
    public void 매치코어_생성시_퍼블릭_의존성_객체들이_초기화된다()
    {
        MatchCore matchCore = CreateSut();

        Assert.IsNotNull(matchCore.PhaseManager);
        Assert.IsNotNull(matchCore.BanPickHandler);
        Assert.IsNotNull(matchCore.SkillController);
        Assert.IsNotNull(matchCore.MasteryRegistry);
        Assert.IsNotNull(matchCore.PhaseEventDispatcher);
    }

    [Test]
    public void 게임이_끝나면_결과를_계산하여_이벤트를_발생시킨다()
    {
        MatchCore core = CreateSut();
        bool isCall = false;
        core.OnMatchFinished += result => isCall = true;

        core.PhaseEventDispatcher.Dispatch(GamePhase.Done, Team.All);

        Assert.IsTrue(isCall);
    }

    MatchCore CreateSut()
    {
        var blueMastery = new MasteryStatCollection(new ChampionMastery[0]);
        var redMastery = new MasteryStatCollection(new ChampionMastery[0]);
        var matchCore = new MatchCore(CreateCaltalog(), CreateStorage(2), CreatePhaseAdvancer(), new MasteryRegistry(blueMastery, redMastery), CreateTeamBonus());

        var blueEntry = new DummyPhaseEntry(Team.Blue);
        var redEntry = new DummyPhaseEntry(Team.Red);
        matchCore.SetupPhaseManager(blueEntry, redEntry);
        return matchCore;
    }

    class DummyPhaseEntry : IPhaseEntry
    {
        public Team Team { get; }
        public DummyPhaseEntry(Team team) => Team = team;
        public void EnterBan() { }
        public void EnterPick() { }
    }
}