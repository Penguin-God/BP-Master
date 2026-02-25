using NUnit.Framework;

public class LeagueScheduleUsecaseTest
{
    class FakeStorage : IScheduleStorage
    {
        public int SavedIndex;
        public void SaveIndex(int index) => SavedIndex = index;
    }

    class FakeSceneLoader : ISceneLoader
    {
        public bool IsLoaded;
        public MatchData LoadedMatch;
        public void LoadBattleScene(MatchData match)
        {
            IsLoaded = true;
            LoadedMatch = match;
        }
    }

    class FakeAiResolver : IAiBattleResolver
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
        public FakeStorage Storage;
        public FakeSceneLoader SceneLoader;
        public FakeAiResolver AiResolver;
    }

    TestContext CreateUsecase(MatchData[] matches, int playerId, int startIndex = 0)
    {
        var flow = new ScheduleFlow(matches);
        var storage = new FakeStorage();
        var sceneLoader = new FakeSceneLoader();
        var aiResolver = new FakeAiResolver();

        var usecase = new LeagueScheduleUsecase(flow, playerId, startIndex, storage, sceneLoader, aiResolver);

        return new TestContext
        {
            Usecase = usecase,
            Storage = storage,
            SceneLoader = sceneLoader,
            AiResolver = aiResolver
        };
    }

    [Test]
    public void 플레이어_매치는_씬을_이동하고_인덱스를_저장한다()
    {
        var matches = new[] { new MatchData(1, 100) };
        var context = CreateUsecase(matches, playerId: 1);

        context.Usecase.ProcessNextMatch();

        Assert.AreEqual(1, context.Storage.SavedIndex);
        Assert.IsTrue(context.SceneLoader.IsLoaded);
        Assert.IsFalse(context.AiResolver.IsResolved);
    }

    [Test]
    public void AI매치는_AI배틀을_진행하고_인덱스를_저장한다()
    {
        var matches = new[] { new MatchData(100, 200) };
        var context = CreateUsecase(matches, playerId: 1);

        context.Usecase.ProcessNextMatch();

        Assert.AreEqual(1, context.Storage.SavedIndex);
        Assert.IsFalse(context.SceneLoader.IsLoaded);
        Assert.IsTrue(context.AiResolver.IsResolved);
    }

    [Test]
    public void 시작_인덱스는_매치를_진행할때마다_증가한다()
    {
        var matches = new[] { new MatchData(1, 100), new MatchData(100, 200) };
        var context = CreateUsecase(matches, playerId: 1, startIndex: 2);

        context.Usecase.ProcessNextMatch();
        Assert.AreEqual(3, context.Storage.SavedIndex);

        context.Usecase.ProcessNextMatch();
        Assert.AreEqual(4, context.Storage.SavedIndex);
    }
}