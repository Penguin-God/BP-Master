using NUnit.Framework;

public class LeagueScheduleUsecaseTest
{
    class FakeResolver : IBattleResolver
    {
        public bool IsResolved;
        public MatchData ResolvedMatch;
        public void Resolve(MatchData match)
        {
            IsResolved = true;
            ResolvedMatch = match;
        }
    }

    class TestContext
    {
        public LeagueScheduleUsecase Usecase;
        public FakeResolver UserResolver;
        public FakeResolver AiResolver;
    }

    TestContext CreateUsecase(MatchData[] matches, int playerId, int startIndex = 0)
    {
        var flow = new ScheduleFlow(matches, startIndex);
        var sceneLoader = new FakeResolver();
        var aiResolver = new FakeResolver();

        var usecase = new LeagueScheduleUsecase(flow, playerId, sceneLoader, aiResolver);

        return new TestContext
        {
            Usecase = usecase,
            UserResolver = sceneLoader,
            AiResolver = aiResolver
        };
    }

    [Test]
    public void 플레이어_매치는_씬을_이동하고_인덱스를_저장한다()
    {
        var matches = new[] { new MatchData(1, 100), new MatchData(1, 200) };
        var context = CreateUsecase(matches, playerId: 1, startIndex: 1);

        context.Usecase.ProcessNextMatch();

        Assert.IsTrue(context.UserResolver.IsResolved);
        Assert.IsFalse(context.AiResolver.IsResolved);
    }

    [Test]
    public void AI매치는_AI배틀을_진행하고_인덱스를_저장한다()
    {
        var matches = new[] { new MatchData(100, 200) };
        var context = CreateUsecase(matches, playerId: 1);

        context.Usecase.ProcessNextMatch();

        Assert.IsFalse(context.UserResolver.IsResolved);
        Assert.IsTrue(context.AiResolver.IsResolved);
    }
}