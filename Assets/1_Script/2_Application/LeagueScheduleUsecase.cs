public interface IScheduleStorage
{
    void SaveIndex(int index);
    int LoadIndex();
}

public interface ISceneLoader
{
    void LoadBattleScene(MatchData match);
}

public interface IAiBattleResolver
{
    void Resolve(MatchData match);
}

public class LeagueScheduleUsecase
{
    readonly ScheduleFlow _flow;
    readonly int _playerId;
    readonly IScheduleStorage _storage;
    readonly ISceneLoader _sceneLoader;
    readonly IAiBattleResolver _aiResolver;

    int _currentIndex;

    public LeagueScheduleUsecase(ScheduleFlow flow, int playerId, IScheduleStorage storage, ISceneLoader sceneLoader, IAiBattleResolver aiResolver)
    {
        _flow = flow;
        _playerId = playerId;
        _currentIndex = storage.LoadIndex();
        _storage = storage;
        _sceneLoader = sceneLoader;
        _aiResolver = aiResolver;
    }

    public void ProcessNextMatch()
    {
        if (_flow.IsFinished) return;

        var currentMatch = _flow.Advance();

        _currentIndex++;
        _storage.SaveIndex(_currentIndex);

        if (IsPlayerMatch(currentMatch))
            _sceneLoader.LoadBattleScene(currentMatch);
        else
            _aiResolver.Resolve(currentMatch);
    }

    bool IsPlayerMatch(MatchData match) => match.Id1 == _playerId || match.Id2 == _playerId;
}